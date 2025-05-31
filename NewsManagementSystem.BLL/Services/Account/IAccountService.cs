using BusinessObject.Entities;

namespace NewsManagementSystem.BLL.Services.Account;

public interface IAccountService
{
    SystemAccount Authenticate(string email, string password);
}
