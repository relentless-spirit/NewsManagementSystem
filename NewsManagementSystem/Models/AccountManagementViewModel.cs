using BusinessObject.Entities;

namespace NewsManagementSystem.Models;

public class AccountManagementViewModel
{
    public List<SystemAccount> Accounts { get; set; }
    public CreateAccountRequest CreateAccountRequest { get; set; }
}