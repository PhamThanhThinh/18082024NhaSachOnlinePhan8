using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NhaSachOnline.InterfaceDungChung;
using NhaSachOnline.Models.DTOs;
using NhaSachOnline.Repositories;

namespace NhaSachOnline.Controllers
{
  public class BookController : Controller
  {
    private readonly IBookRepository _bookRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IFileService _fileService;

    public BookController(IBookRepository bookRepository, IGenreRepository genreRepository, IFileService fileService)
    {
      _bookRepository = bookRepository;
      _genreRepository = genreRepository;
      _fileService = fileService;
    }

    public async Task<IActionResult> Index()
    {
      var books = await _bookRepository.GetBooks();
      return View(books);
    }

    public async Task<IActionResult> AddBook()
    {
      // var genreSelectList
      var chonTheLoaiTrongDanhSachTheLoai = (await _genreRepository.GetGenres()).Select(theLoai => new SelectListItem
      {
        Text = theLoai.GenreName,
        Value = theLoai.Id.ToString()
      }
      );
      BookDTO bookDTO = new()
      {
        GenreList = chonTheLoaiTrongDanhSachTheLoai
      };

      return View(bookDTO);
    }

  }
}
