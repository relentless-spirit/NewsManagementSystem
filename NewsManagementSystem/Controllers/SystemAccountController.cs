using Microsoft.AspNetCore.Mvc;
using NewsManagementSystem.BLL.Services.SystemAccount;

namespace NewsManagementSystem.Controllers
{
    public class SystemAccountController : Controller
    {
        private readonly ISystemAccountService _systemAccountService;

        public SystemAccountController(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }
        public async Task<IActionResult> Accounts()
        {
            var accounts = await _systemAccountService.GetSystemAccountsAsync();
            return View(accounts);
        }


    }
}
