namespace NhaSachOnline.InterfaceDungChung
{
  public interface IFileService
  {
    void DeleteFile(string fileName);
    Task<string> SaveFile(IFormFile formFile, string[] chophepTienIchMoRong);
  }

  public class FileService : IFileService
  {
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileService(IWebHostEnvironment webHostEnvironment)
    {
      _webHostEnvironment = webHostEnvironment;
    }

    public void DeleteFile(string fileName)
    {
      // assets
      // wwwPath đường dẫn tới wwwroot
      var wwwPath = _webHostEnvironment.WebRootPath;
      // tệp tin cùng với đường dẫn
      var teptincungvoiduongdan = Path.Combine(wwwPath, "img\\", fileName);
      if (File.Exists(teptincungvoiduongdan))
      {
        throw new FileNotFoundException(fileName);
      }
      // vượt qua ngoại lệ
      // ta có file cần xóa
      File.Delete(teptincungvoiduongdan);
    }

    public async Task<string> SaveFile(IFormFile formFile, string[] chophepTienIchMoRong)
    {
      var wwwPath = _webHostEnvironment.WebRootPath;
      var path = Path.Combine(wwwPath, "img");

      // cấu hình đường dẫn
      //if (!Directory.Exists(path))
      //{
      //  Directory.CreateDirectory(path);
      //}
      if (Directory.Exists(path) == false)
      {
        Directory.CreateDirectory(path);
      }

      // xử lý ngoại lệ
      var tienIchMoRong = Path.GetExtension(formFile.FileName);
      if (chophepTienIchMoRong.Contains(tienIchMoRong) == false)
      {
        throw new InvalidOperationException($" Chỉ cho phép file {string.Join(",", chophepTienIchMoRong)}");
      }
      string fileName = Path.GetFileName(formFile.FileName);
      return fileName;
    }
  }
}
