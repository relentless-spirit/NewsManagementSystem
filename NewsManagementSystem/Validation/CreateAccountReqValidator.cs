using FluentValidation;
using NewsManagementSystem.BLL.Services.SystemAccount;
using NewsManagementSystem.Models;

namespace NewsManagementSystem.Validation;

public class CreateAccountReqValidator : AbstractValidator<CreateAccountRequest>
{
    private readonly ISystemAccountService _systemAccountService;
    
    public CreateAccountReqValidator(ISystemAccountService systemAccountService)
    {
        _systemAccountService = systemAccountService;
        
        RuleFor(x => x.AccountID)
            .NotNull().WithMessage("Please provide a valid ID")
            .MustAsync(BeUniqueAccountId).WithMessage("Account ID already exists");
        RuleFor(x => x.AccountName)
            .NotEmpty().WithMessage("Name is required")
            .Length(5, 100).WithMessage("Name must be between 5 and 100 characters");
        RuleFor(x => x.AccountEmail)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("The input is not a valid email address");
        RuleFor(x => x.AccountRole)
            .NotEmpty().WithMessage("Role is required");
        RuleFor(x => x.AccountPassword)
            .NotEmpty().WithMessage("Password is required").MinimumLength(5).WithMessage("Password must be greater than 5 characters");
    }
    
    private async Task<bool> BeUniqueAccountId(short accountId, CancellationToken cancellationToken)
    {
        var account = await _systemAccountService.GetSystemAccountByIdAsync(accountId);
        return account == null;
    }
}