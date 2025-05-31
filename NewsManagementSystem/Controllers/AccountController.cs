using Microsoft.AspNetCore.Mvc;
using NewsManagementSystem.BLL.Services.Account;
using BusinessObject.Entities;

namespace NewsManagementSystem.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        var user = _accountService.Authenticate(email, password);
        if (user != null)
        {
            HttpContext.Session.SetString("UserName", user.AccountName);
            HttpContext.Session.SetString("Email", user.AccountEmail);
            HttpContext.Session.SetInt32("Role", user.AccountRole ?? -1);
            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Email hoặc mật khẩu không đúng.";
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
