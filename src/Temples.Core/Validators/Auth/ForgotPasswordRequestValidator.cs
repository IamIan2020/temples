using FluentValidation;
using Temples.Core.DTOs.Auth;

namespace Temples.Core.Validators.Auth;

public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email 為必填")
            .EmailAddress().WithMessage("Email 格式不正確");
    }
}
