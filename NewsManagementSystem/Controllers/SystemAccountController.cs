using AutoMapper;
using BusinessObject.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using NewsManagementSystem.BLL.Services.SystemAccount;
using NewsManagementSystem.Models;

namespace NewsManagementSystem.Controllers
{
    public class SystemAccountController : Controller
    {
        private readonly ISystemAccountService _systemAccountService;
        private readonly IValidator<CreateAccountRequest> _createAccountRequestValidator;
        private readonly IMapper _mapper;

        public SystemAccountController(ISystemAccountService systemAccountService, IValidator<CreateAccountRequest> createAccountRequestValidator, IMapper mapper)
        {
            _systemAccountService = systemAccountService;
            _createAccountRequestValidator = createAccountRequestValidator;
            _mapper = mapper;
        }
        public async Task<IActionResult> Accounts()
        {
            var accounts = await _systemAccountService.GetSystemAccountsAsync();
            var viewModel = new AccountManagementViewModel() { Accounts = accounts };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult CreateSystemAccount()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateSystemAccount(CreateAccountRequest accountRequest)
        {
            ValidationResult result = await _createAccountRequestValidator.ValidateAsync(accountRequest);
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return View("CreateSystemAccount", accountRequest);
            }
            var systemAccount = _mapper.Map<SystemAccount>(accountRequest);
            await _systemAccountService.CreateSystemAccountAsync(systemAccount);
            return RedirectToAction("Accounts");
        }

    }
}
