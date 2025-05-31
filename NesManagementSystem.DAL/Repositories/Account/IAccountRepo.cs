using BusinessObject.Entities;

namespace NewsManagementSystem.DAL.Repositories.Account
{
    public interface IAccountRepo
    {
     
        public SystemAccount GetAccountByEmailAndPassword(string email, string password);
    }
}
