using FluentValidation;
using Temples.Core.DTOs.Members;

namespace Temples.Core.Validators.Members;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("目前密碼為必填");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("新密碼為必填")
            .MinimumLength(8).WithMessage("新密碼至少需要 8 個字元");
    }
}
