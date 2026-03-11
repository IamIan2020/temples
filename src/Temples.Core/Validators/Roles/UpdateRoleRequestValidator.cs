using FluentValidation;
using Temples.Core.Constants;
using Temples.Core.DTOs.Roles;

namespace Temples.Core.Validators.Roles;

public class UpdateRoleRequestValidator : AbstractValidator<UpdateRoleRequest>
{
    public UpdateRoleRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("角色名稱為必填")
            .MaximumLength(100).WithMessage("角色名稱不得超過 100 字元");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("描述不得超過 500 字元");

        RuleForEach(x => x.Permissions)
            .Must(p => Permissions.All.Contains(p))
            .WithMessage("無效的權限代碼：{PropertyValue}");
    }
}
