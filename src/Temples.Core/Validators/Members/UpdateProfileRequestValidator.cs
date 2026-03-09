using FluentValidation;
using Temples.Core.DTOs.Members;

namespace Temples.Core.Validators.Members;

public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("顯示名稱為必填")
            .MaximumLength(100).WithMessage("顯示名稱不可超過 100 個字元");

        RuleFor(x => x.ChineseName)
            .MaximumLength(50).WithMessage("中文姓名不可超過 50 個字元")
            .When(x => x.ChineseName != null);

        RuleFor(x => x.Gender)
            .MaximumLength(10).WithMessage("性別不可超過 10 個字元")
            .When(x => x.Gender != null);

        RuleFor(x => x.Address)
            .MaximumLength(500).WithMessage("地址不可超過 500 個字元")
            .When(x => x.Address != null);
    }
}
