using FluentValidation;
using Temples.Core.DTOs.Auth;

namespace Temples.Core.Validators.Auth;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email 為必填")
            .EmailAddress().WithMessage("Email 格式不正確");

        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token 為必填");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("新密碼為必填")
            .MinimumLength(8).WithMessage("密碼至少需要 8 個字元");
    }
}
