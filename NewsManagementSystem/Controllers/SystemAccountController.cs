using AutoMapper;
using BusinessObject.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewsManagementSystem.BLL.Services.SystemAccount;
using NewsManagementSystem.Models;
using Microsoft.AspNetCore.Http;

namespace NewsManagementSystem.Controllers
{
    public class SystemAccountController : Controller
    {
        private readonly ISystemAccountService _systemAccountService;
        private readonly IValidator<CreateAccountRequest> _createAccountRequestValidator;
        private readonly IValidator<UpdateSystemAccountRequest> _updateSystemAccountRequestValidator;
        private readonly IMapper _mapper;

        public SystemAccountController(
            ISystemAccountService systemAccountService,
            IValidator<CreateAccountRequest> createAccountRequestValidator,
            IMapper mapper,
            IValidator<UpdateSystemAccountRequest> updateAccountRequestValidator)
        {
            _systemAccountService = systemAccountService;
            _createAccountRequestValidator = createAccountRequestValidator;
            _mapper = mapper;
            _updateSystemAccountRequestValidator = updateAccountRequestValidator;
        }

        // ✅ LOGIN (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // ✅ LOGIN (POST)
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _systemAccountService.AuthenticateAsync(email, password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("ID", user.AccountID);
                HttpContext.Session.SetString("UserName", user.AccountName ?? "");
                HttpContext.Session.SetString("Email", user.AccountEmail);
                HttpContext.Session.SetInt32("Role", user.AccountRole ?? -1);
                var role = HttpContext.Session.GetInt32("Role");
                if (role == 1)
                {
                    return RedirectToAction("ListCategories", "Category");
                }
                else if (role == 2)
                {
                    return RedirectToAction("GetArticlesync", "Article");
                }
                return RedirectToAction("Accounts", "SystemAccount"); // Redirect sau login
            }

            ViewBag.Error = "Email hoặc mật khẩu không đúng.";
            return View();
        }

        // ✅ LOGOUT
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Accounts(string? search)
        {
            List<SystemAccount> accounts;
            var role = HttpContext.Session.GetInt32("Role");
            if (role != 0) // ✅ Chỉ Admin
                return RedirectToAction("AccessDenied", "Home");
            if (search == null)
            {
                accounts = await _systemAccountService.GetSystemAccountsAsync();
            }
            else
            {
                accounts = await _systemAccountService.GetSystemAccountByNameAsync(search);
            }
            ViewBag.Search = search;
            var viewModel = new AccountManagementViewModel() { Accounts = accounts };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSystemAccount([FromBody] CreateAccountRequest accountRequest)
        {
            var role = HttpContext.Session.GetInt32("Role");
            if (role != 0)
                return Unauthorized();

            ValidationResult result = await _createAccountRequestValidator.ValidateAsync(accountRequest);
            if (!result.IsValid)
            {
                var errors = result.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(new { success = false, errors });
            }

            var systemAccount = _mapper.Map<SystemAccount>(accountRequest);
            await _systemAccountService.CreateSystemAccountAsync(systemAccount);
            return Ok(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] UpdateSystemAccountRequest account)
        {
            var role = HttpContext.Session.GetInt32("Role");
            // if (role != 0 )
            //     return Unauthorized();

            ValidationResult result = await _updateSystemAccountRequestValidator.ValidateAsync(account);
            if (!result.IsValid)
            {
                var errors = result.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(new { success = false, errors });
            }

            var systemAccount = _mapper.Map<SystemAccount>(account);
            await _systemAccountService.UpdateSystemAccountAsync(systemAccount);
            return Ok(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(short id)
        {
            var role = HttpContext.Session.GetInt32("Role");
            if (role != 0)
                return Unauthorized();

            var account = await _systemAccountService.GetSystemAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            await _systemAccountService.DeleteSystemAccountAsync(account);
            return RedirectToAction("Accounts");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var role = HttpContext.Session.GetInt32("Role");
            if (role == 0)
                return RedirectToAction("AccessDenied", "Home");
            var id = (short)(HttpContext.Session.GetInt32("ID") ?? 0);
            var account = await _systemAccountService.GetSystemAccountByIdAsync(id);
            if (account == null)
            {
                return View("Login");
            }
            var model = new UpdateSystemAccountRequest()
            {
                AccountID = account.AccountID,
                AccountName = account.AccountName,
                AccountEmail = account.AccountEmail,
                AccountRole = account.AccountRole,
                AccountPassword = account.AccountPassword,
            };
            var profile = new AccountManagementViewModel() { UpdateSystemAccountRequest = model };
            return View(profile);
        }

    }
}
