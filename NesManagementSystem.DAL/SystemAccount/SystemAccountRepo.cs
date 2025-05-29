using Microsoft.EntityFrameworkCore;
using NewsManagementSystem.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.DAL.SystemAccount
{
    public class SystemAccountRepo : ISystemAccountRepo
    {
        private readonly FUNewsManagementContext _context;
        public SystemAccountRepo(FUNewsManagementContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public Task CreateSystemAccountAsync(BusinessObject.Entities.SystemAccount systemAccount)
        {
            throw new NotImplementedException();
        }

        public Task<BusinessObject.Entities.SystemAccount?> GetSystemAccountByNameAsync(string systemAccountName)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BusinessObject.Entities.SystemAccount>> GetSystemAccountsAsync()
        {
            return await _context.SystemAccounts.ToListAsync();
        }
    }
}
