using AutoMapper;
using BusinessObject.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewsManagementSystem.BLL.Services.SystemAccount;
using NewsManagementSystem.Models;

namespace NewsManagementSystem.Controllers
{
    public class SystemAccountController : Controller
    {
        private readonly ISystemAccountService _systemAccountService;
        private readonly IValidator<CreateAccountRequest> _createAccountRequestValidator;
        private readonly IValidator<UpdateSystemAccountRequest> _updateSystemAccountRequestValidator;
        private readonly IMapper _mapper;

        public SystemAccountController(ISystemAccountService systemAccountService, IValidator<CreateAccountRequest> createAccountRequestValidator, IMapper mapper, IValidator<UpdateSystemAccountRequest> updateAccountRequestValidator)
        {
            _systemAccountService = systemAccountService;
            _createAccountRequestValidator = createAccountRequestValidator;
            _mapper = mapper;
            _updateSystemAccountRequestValidator = updateAccountRequestValidator;
        }
        public async Task<IActionResult> Accounts()
        {
            var accounts = await _systemAccountService.GetSystemAccountsAsync();
            var viewModel = new AccountManagementViewModel() { Accounts = accounts };
            return View(viewModel);
        }

        // [HttpGet]
        // public IActionResult CreateSystemAccount()
        // {
        //     return View();
        // }


        [HttpPost]
        public async Task<IActionResult> CreateSystemAccount([FromBody]CreateAccountRequest accountRequest)
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
            return Ok(new {success = true});
        }

        // GET: Edit
        //public async Task<IActionResult> Edit(short id)
        //{
        //    if (id <= 0)
        //        return BadRequest("Invalid account ID.");

        //    var account = await _systemAccountService.GetSystemAccountByIdAsync(id);
        //    if (account == null)
        //        return NotFound("Account not found.");

        //    return View(account);
        //}

        // POST: Edit (Update Account from modal)
        [HttpPost]

        public async Task<IActionResult> Edit([FromBody]UpdateSystemAccountRequest account)
        {

            ValidationResult result = await _updateSystemAccountRequestValidator.ValidateAsync(account);
            if (!result.IsValid)
            {
                // Lấy lại danh sách accounts để render lại view
                // var accounts = await _systemAccountService.GetSystemAccountsAsync();
                // var viewModel = new AccountManagementViewModel
                // {
                //     Accounts = accounts,
                //     UpdateSystemAccountRequest = account
                // };
                // Đặt cờ để tự động mở lại modal khi có lỗi
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
            // if (!ModelState.IsValid)
            // {
            //     // Lấy lại danh sách accounts để render lại view
            //     var accounts = await _systemAccountService.GetSystemAccountsAsync();
            //     var viewModel = new AccountManagementViewModel
            //     {
            //         Accounts = accounts,
            //         UpdateSystemAccountRequest = account
            //     };
            //     ViewBag.ShowUpdateModal = true;
            //     return View("Accounts", viewModel);
            // }
            //
            // // Map sang entity và update
            // var systemAccount = _mapper.Map<SystemAccount>(account);
            // await _systemAccountService.UpdateSystemAccountAsync(systemAccount);
            //
            // return RedirectToAction("Accounts");
        }
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