using NewsManagementSystem.DAL.SystemAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.BLL.Services.SystemAccount
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly  ISystemAccountRepo _systemAccountRepo;
          public SystemAccountService(ISystemAccountRepo systemAccountRepo)
        {
            _systemAccountRepo = systemAccountRepo;
        }

        public async Task CreateSystemAccountAsync(BusinessObject.Entities.SystemAccount systemAccount)
        {
            await _systemAccountRepo.CreateSystemAccountAsync(systemAccount);
        }

        public async Task<BusinessObject.Entities.SystemAccount?> GetSystemAccountByIdAsync(short id)
        {
            return await _systemAccountRepo.GetSystemAccountByIdAsync(id);
        }

        public Task<BusinessObject.Entities.SystemAccount> GetSystemAccountByNameAsync(string systemAccountName)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BusinessObject.Entities.SystemAccount>> GetSystemAccountsAsync()
        {
            return await _systemAccountRepo.GetSystemAccountsAsync();
        }
    }

}
