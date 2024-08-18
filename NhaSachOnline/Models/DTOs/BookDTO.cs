using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace NhaSachOnline.Models.DTOs
{
  public class BookDTO
  {
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string? BookName { get; set; }

    [Required]
    [MaxLength(50)]
    public string? AuthorName { get; set; }

    [Required]
    public double Price { get; set; }

    public string? Image { get; set; }

    [Required]
    public int GenreId { get; set; }

    // ImageFile tương đương với một ảnh được tải lên
    // qua 1 form trong trang web
    // IFormFile gửi một yêu cầu
    // Khi người dùng tải lên
    // một hình ảnh (ImageFile) lưu vào máy chủ
    public IFormFile? ImageFile { get; set; }
    public IEnumerable<SelectListItem>? GenreList { get; set; }

    // Những thông tin Thịnh khai báo ở đây sẽ được
    // lưu vào máy chủ
  }
}
