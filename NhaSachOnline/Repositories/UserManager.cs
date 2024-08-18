using Microsoft.AspNetCore.Identity;

namespace NhaSachOnline.Repositories
{
  public interface IUserManager
  {
    string UserName { get; }
  }
  public class UserManager : IUserManager
  {
    public string UserName => throw new NotImplementedException();
  }
}