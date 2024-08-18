using NhaSachOnline.Models;

namespace NhaSachOnline
{
  public interface IUserManager
  {
    Task<IEnumerable<Order>> UserOrders(bool getAll = false);
    Task<Order?> GetOrderById(int id);
    Task<IEnumerable<OrderStatus>> GetOrderStatus();
    Task TogglePaymentStatus (int orderid);
    //Task ChangeOrderStatus()
  }
}