using FluentValidation;
using Temples.Core.DTOs.Auth;

namespace Temples.Core.Validators.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email 為必填")
            .EmailAddress().WithMessage("Email 格式不正確");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("密碼為必填")
            .MinimumLength(8).WithMessage("密碼至少需要 8 個字元");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("確認密碼為必填")
            .Equal(x => x.Password).WithMessage("確認密碼與密碼不一致");

        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("顯示名稱為必填")
            .MaximumLength(100).WithMessage("顯示名稱不可超過 100 個字元");
    }
}
