using FluentValidation;
using Temples.Core.DTOs.Members;

namespace Temples.Core.Validators.Members;

public class ChangeRoleRequestValidator : AbstractValidator<ChangeRoleRequest>
{
    private static readonly string[] ValidRoles = ["SystemAdmin", "WebAdmin", "Member"];

    public ChangeRoleRequestValidator()
    {
        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("角色為必填")
            .Must(role => ValidRoles.Contains(role))
            .WithMessage("角色必須是 SystemAdmin、WebAdmin 或 Member");
    }
}
