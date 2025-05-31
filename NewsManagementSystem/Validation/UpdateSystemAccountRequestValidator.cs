using FluentValidation;
using NewsManagementSystem.BLL.Services.SystemAccount;
using NewsManagementSystem.Models;

namespace NewsManagementSystem.Validation
{
    public class UpdateSystemAccountRequestValidator : AbstractValidator<UpdateSystemAccountRequest>
    {
        
             private readonly ISystemAccountService _systemAccountService;

        public UpdateSystemAccountRequestValidator(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;

            RuleFor(x => x.AccountName)
      .NotEmpty().WithMessage("Name is required")
      .Length(5, 100).WithMessage("Name must be between 5 and 100 characters")
      .Matches(@"^[a-zA-Z\s]+$").WithMessage("Name must not contain numbers or special characters");
RuleFor(x => x.AccountEmail)
    .NotEmpty().WithMessage("Email is required")
    .EmailAddress().WithMessage("The input is not a valid email address");

RuleFor(x => x.AccountRole)
    .NotEmpty().WithMessage("Role is required");

RuleFor(x => x.AccountPassword)
    .NotEmpty().WithMessage("Password is required")
    .MinimumLength(6).WithMessage("Password must be at least 6 characters")
    .MaximumLength(50).WithMessage("Password must not exceed 50 characters");
        }

    }
}

