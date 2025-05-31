using BusinessObject.Entities;
using NewsManagementSystem.DAL.DBContext;

namespace NewsManagementSystem.DAL.Repositories.Account;

public class AccountRepo : IAccountRepo
{
    private readonly FUNewsManagementContext _context;

    public AccountRepo(FUNewsManagementContext context)
    {
        _context = context;
    }

    public SystemAccount GetAccountByEmailAndPassword(string email, string password)
    {
        return _context.SystemAccounts
            .FirstOrDefault(a => a.AccountEmail == email && a.AccountPassword == password);
    }
}
