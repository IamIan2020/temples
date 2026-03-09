using FluentValidation;
using Temples.Core.DTOs.Auth;

namespace Temples.Core.Validators.Auth;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email 為必填")
            .EmailAddress().WithMessage("Email 格式不正確");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("密碼為必填");
    }
}
