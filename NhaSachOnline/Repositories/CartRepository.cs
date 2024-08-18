using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using NhaSachOnline.Data;
using NhaSachOnline.Models;
using NhaSachOnline.Models.DTOs;
using System.Net;

namespace NhaSachOnline.Repositories
{
  public class CartRepository : ICartRepository
  {
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IHttpContextAccessor _contextAccessor;
    private UserManager<IdentityUser>? userManager;

    //public CartRepository(ApplicationDbContext dbContext, 
    //  IHttpContextAccessor iHttpContextAccessor, 
    //  UserManager<IdentityUser userManager>)
    //{
    //  _dbContext = dbContext;
    //  _userManager = userManager;
    //  _contextAccessor = iHttpContextAccessor;
    //}

    public Task<int> AddItem(int bookId, int soluong)
    {
      throw new UnauthorizedAccessException("Người dùng chưa đăng nhập");
    }

    public async Task<bool> DoCheckout(CheckoutModel model)
    {
      using var transaction = _dbContext.Database.BeginTransaction();

      try
      {
        var userId = GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
          throw new UnauthorizedAccessException("Người dùng chưa đăng nhập");
        }

        var cart = await GetCart(userId);
        if (cart is null)
        {
          throw new InvalidOperationException("lỗi, giỏ hàng trống");
        }

        var chitietgiohang = _dbContext.CartDetails
          .Where(i => i.ShoppingCartId == cart.Id).ToList();
        if (chitietgiohang.Count == 0)
        {
          throw new InvalidOperationException("giỏ hàng trống");
        }

        var trangthaidonhang = _dbContext.OrderStatuses
          .FirstOrDefault(s => s.StatusName == "Pending");
        if (trangthaidonhang is null)
        {
          throw new InvalidOperationException("đơn hàng đang chờ xử lý");
        }

        var order = new Order
        {
          UserId = userId,
          CreateDate = DateTime.UtcNow,
          Name = model.Name,
          Email = model.Email,
          MobileNumber = model.MobileNumber,
          PaymentMethod = model.PaymentMethod,
          Address = model.Address,
          IsPaid = false,
          //OrderStatus = trangthaidonhang.Id
        };

        _dbContext.Orders.Add(order);
        _dbContext.SaveChanges();

        foreach (var item in chitietgiohang)
        {
          var chitietDonHang = new OrderDetail
          {
            BookId = item.BookId,
            OrderId = order.Id,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
          };
          //_dbContext.OrderDetails.Add();
        }


      }
      catch
      {

      }

      throw new NotImplementedException();
    }

    //public async Task<int> AddItem(int bookId, int soluong)
    //{
    //  _dbContext.SaveChangesAsync();
    //}

    public async Task<ShoppingCart> GetCart(string userId)
    {
      var cart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(u => u.UserId == userId);
      return cart;
    }

    public async Task<int> GetCartItemCount(string userId = "")
    {
      // cập nhật
      if (string.IsNullOrEmpty(userId))
      {
        userId = GetUserId();
      }
      var data = await (
        from cart in _dbContext.ShoppingCarts
        join cartDetail in _dbContext.CartDetails
        on cart.Id equals cartDetail.ShoppingCartId
        where cart.UserId == userId // cập nhật
        select new { cartDetail.Id }
        ).ToListAsync();
      return data.Count();
    }

    public async Task<ShoppingCart> GetUserCart(int id)
    {
      var userId = GetUserId();
      if (userId == null)
      {
        throw new InvalidOperationException("khong the tim thay id");
      }
      //var shoppingCart = await _dbContext.ShoppingCarts
      //  .Include(cart => cart.CartDetails)
      //  .ThenInclude(book => book.Book)

      var shoppingCart = await _dbContext.ShoppingCarts
        .Include(cart => cart.CartDetails)
        .ThenInclude(book => book.Book)
        .ThenInclude(stock => stock.Stock)
        .Include(cartdetail => cartdetail.CartDetails)
        .ThenInclude(book => book.Book)
        .ThenInclude(book => book.Genre)
        .Where(userid => userid.UserId == userId).FirstOrDefaultAsync();

      // stock liên quan tới việc lưu trữ hàng hóa trong kho
      return shoppingCart;
    }

    public async Task<int> RemoveItem(int bookId)
    {
      string userId = GetUserId();
      // xử lý ngoại lệ
      try
      {
        if (string.IsNullOrEmpty(userId))
        {
          throw new UnauthorizedAccessException("bạn chưa đăng nhập");
        }
        
        var cart = await GetCart(userId);
        if (cart is null)
        {
          throw new UnauthorizedAccessException("giỏ hàng trống");
        }

        // shopping phải đồng bộ trong nguyên một đoạn code:
        // shopping => shopping.ShoppingCartId == cart.Id && shopping.BookId == bookId
        var cartItem = _dbContext.CartDetails
          .FirstOrDefault(shopping => shopping.ShoppingCartId == cart.Id && shopping.BookId == bookId);

        if (cartItem is null)
        {
          throw new InvalidOperationException("không có item nào trong giỏ hàng");
        }
        else if (cartItem.Quantity == 1)
        {
          _dbContext.CartDetails.Remove(cartItem);
        }
        else
        {
          // cartItem.Quantity số lượng item không giảm xuống dưới 0, gây lỗi
          cartItem.Quantity = cartItem.Quantity - 1;
        }
        _dbContext.SaveChanges();
      }
      catch (Exception ex)
      {
        // đối với các lỗi không mong muốn, ném nó vào đây
        throw new UnauthorizedAccessException("lỗi, vui lòng chạy lại");
      }

      var cartItemCount = await GetCartItemCount(userId);

      return cartItemCount;
    }

    private string GetUserId()
    {
      // Nhận diện người dùng
      var nhandiennguoidung = _contextAccessor.HttpContext.User;
      string userId = _userManager.GetUserId(nhandiennguoidung);
      return userId;

      // Nhận diện người dùng

      //var httpContext = _contextAccessor.HttpContext;

      //// kiểm tra
      //if (httpContext?.User != null)
      //{
      //  return _userManager.GetUserId(httpContext.User);
      //}

      //return null;
    }

  }
}
