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
                HttpContext.Session.SetString("UserName", user.AccountName ?? "");
                HttpContext.Session.SetString("Email", user.AccountEmail);
                HttpContext.Session.SetInt32("Role", user.AccountRole ?? -1);

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

        // ✅ DANH SÁCH ACCOUNT
        public async Task<IActionResult> Accounts()
        {
            var accounts = await _systemAccountService.GetSystemAccountsAsync();
            var viewModel = new AccountManagementViewModel() { Accounts = accounts };
            return View(viewModel);
        }

        // ✅ TẠO ACCOUNT (AJAX)
        [HttpPost]
        public async Task<IActionResult> CreateSystemAccount([FromBody] CreateAccountRequest accountRequest)
        {
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

        // ✅ UPDATE ACCOUNT (AJAX)
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] UpdateSystemAccountRequest account)
        {
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

        // ✅ DELETE ACCOUNT
        [HttpPost]
        public async Task<IActionResult> Delete(short id)
        {
            var account = await _systemAccountService.GetSystemAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            await _systemAccountService.DeleteSystemAccountAsync(account);
            return RedirectToAction("Accounts");
        }
    }
}
