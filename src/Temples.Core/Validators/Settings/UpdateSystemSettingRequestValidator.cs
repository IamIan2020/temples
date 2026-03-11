using FluentValidation;
using Temples.Core.DTOs.Settings;

namespace Temples.Core.Validators.Settings;

public class UpdateSystemSettingRequestValidator : AbstractValidator<UpdateSystemSettingRequest>
{
    public UpdateSystemSettingRequestValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("公司名稱為必填")
            .MaximumLength(200).WithMessage("公司名稱不得超過 200 字元");

        RuleFor(x => x.WebsiteName)
            .NotEmpty().WithMessage("網站名稱為必填")
            .MaximumLength(200).WithMessage("網站名稱不得超過 200 字元");

        RuleFor(x => x.Phone)
            .MaximumLength(50).WithMessage("電話不得超過 50 字元");

        RuleFor(x => x.TaxId)
            .MaximumLength(20).WithMessage("統編不得超過 20 字元");

        RuleFor(x => x.Copyright)
            .NotEmpty().WithMessage("Copyright 為必填")
            .MaximumLength(500).WithMessage("Copyright 不得超過 500 字元");

        RuleFor(x => x.SessionTimeoutMinutes)
            .InclusiveBetween(1, 480).WithMessage("登出時間必須在 1 到 480 分鐘之間");
    }
}
