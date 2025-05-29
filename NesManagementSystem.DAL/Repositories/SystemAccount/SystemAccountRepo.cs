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
        public async Task CreateSystemAccountAsync(BusinessObject.Entities.SystemAccount systemAccount)
        {
            await _context.SystemAccounts.AddAsync(systemAccount);
            await _context.SaveChangesAsync();
        }

        public async Task<BusinessObject.Entities.SystemAccount?> GetSystemAccountByIdAsync(short id)
        {
             return await _context.SystemAccounts.FindAsync(id);
        }

        public Task<List<BusinessObject.Entities.SystemAccount?>> GetSystemAccountByNameAsync(string systemAccountName)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BusinessObject.Entities.SystemAccount>> GetSystemAccountsAsync()
        {
            return await _context.SystemAccounts.ToListAsync();
        }
    }
}
