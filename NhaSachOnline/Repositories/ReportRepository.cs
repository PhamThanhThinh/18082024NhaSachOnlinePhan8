using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NhaSachOnline.Data;
using NhaSachOnline.Models;
using NhaSachOnline.Models.DTOs;

namespace NhaSachOnline.Repositories
{
  public interface IReportRepository
  {
    Task<IEnumerable<Book>> LayThongTinBanSanPhamTheoNgay(DateTime ngayBatDau, DateTime ngayKetThuc);
  }

  public class ReportRepository : IReportRepository
  {
    private readonly ApplicationDbContext _dbContext;

    public ReportRepository(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }
    //public async Task<IEnumerable<ResultBookModel>> LayThongTinBanSanPhamTheoNgay(DateTime ngayBatDau, DateTime ngayKetThuc)
    //{
    //  DateTime ngayBatDau = DateTime.Now;
    //  var ngayBatDau = new SqlParameter("@ngayBatDau", ngayBatDau);
    //  var ngayKetThuc = new SqlParameter("@ngayKetThuc", ngayKetThuc);
    //  var result = await _dbContext.Database.SqlQueryRaw<ResultBookModel>
    //    ("exec Usp_GetResultBookByDate @ngayBatDau, @ngayKetThuc", ngayBatDau, ngayKetThuc).ToListAsync();
    //  return result;
    //}

    public async Task<IEnumerable<ResultBookModel>> LayThongTinBanSanPhamTheoNgay(DateTime ngayBatDau, DateTime ngayKetThuc)
    {
      // Khai báo các SqlParameter với giá trị của tham số phương thức (tham số hàm hay tham số truyền vào)
      var paramNgayBatDau = new SqlParameter("@ngayBatDau", ngayBatDau);
      var paramNgayKetThuc = new SqlParameter("@ngayKetThuc", ngayKetThuc);

      // Thực thi câu lệnh SQL và lấy kết quả
      var result = await _dbContext.Database.SqlQueryRaw<ResultBookModel>(
          "exec Usp_GetResultBookByDate @ngayBatDau, @ngayKetThuc",
          paramNgayBatDau, paramNgayKetThuc
      ).ToListAsync();

      return result;
    }

    Task<IEnumerable<Book>> IReportRepository.LayThongTinBanSanPhamTheoNgay(DateTime ngayBatDau, DateTime ngayKetThuc)
    {
      throw new NotImplementedException();
    }
  }
}
