using BusinessObject.Entities;
using Microsoft.Extensions.Configuration;
using NewsManagementSystem.DAL.Repositories.Account;

namespace NewsManagementSystem.BLL.Services.Account;

public class AccountService : IAccountService
{
    private readonly IAccountRepo _repo;
    private readonly IConfiguration _config;

    public AccountService(IAccountRepo repo, IConfiguration config)
    {
        _repo = repo;
        _config = config;
    }

    public SystemAccount Authenticate(string email, string password)
    {
        var adminEmail = _config["AdminAccount:Email"];
        var adminPass = _config["AdminAccount:Password"];

        if (email == adminEmail && password == adminPass)
        {
            return new SystemAccount
            {
                AccountEmail = adminEmail,
                AccountPassword = adminPass,
                AccountRole = 0,
                AccountName = "Admin"
            };
        }

        return _repo.GetAccountByEmailAndPassword(email, password);
    }
}
