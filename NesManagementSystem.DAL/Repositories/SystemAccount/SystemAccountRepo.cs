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

        public async Task DeleteSystemAccountAsync(BusinessObject.Entities.SystemAccount systemAccount)
        {
            //var articles = _context.NewsArticles.Where(a => a.CreatedByID == systemAccount.AccountID).ToList();
            //foreach (var article in articles)
            //{
              
            //    var tags = _context.Tags.Where(t => t.NewsArticles.Any(na => na.NewsArticleID == article.NewsArticleID));
            //    _context.Tags.RemoveRange(tags);

            //    _context.NewsArticles.Remove(article);
            //}

            //_context.SystemAccounts.Remove(systemAccount);
            //await _context.SaveChangesAsync();

            var account = await _context.SystemAccounts.Include(a=>a.NewsArticles).FirstOrDefaultAsync(a => a.AccountID == systemAccount.AccountID);
            if (account == null)
            {
                throw new KeyNotFoundException($"System account with ID {systemAccount.AccountID} not found.");
            }
            account.NewsArticles.Clear(); 
            _context.SystemAccounts.Remove(account); 
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

        public async Task UpdateSystemAccountAsync(BusinessObject.Entities.SystemAccount systemAccount)
        {
            var existingAccount = await _context.SystemAccounts.FindAsync(systemAccount.AccountID);
            if (existingAccount == null)
            {
                throw new KeyNotFoundException($"System account with ID {systemAccount.AccountID} not found.");
            }
            existingAccount.AccountName = systemAccount.AccountName;
            existingAccount.AccountPassword = systemAccount.AccountPassword;
            existingAccount.AccountEmail = systemAccount.AccountEmail;
            existingAccount.AccountRole = systemAccount.AccountRole;

            //_context.SystemAccounts.Update(systemAccount);
            await _context.SaveChangesAsync();
        }
    }
}
