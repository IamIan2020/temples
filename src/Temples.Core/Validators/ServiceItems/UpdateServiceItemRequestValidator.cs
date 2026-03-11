using FluentValidation;
using Temples.Core.DTOs.ServiceItems;

namespace Temples.Core.Validators.ServiceItems;

public class UpdateServiceItemRequestValidator : AbstractValidator<UpdateServiceItemRequest>
{
    public UpdateServiceItemRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("標題為必填")
            .MaximumLength(200).WithMessage("標題不得超過 200 字元");

        RuleFor(x => x.HeaderImage)
            .MaximumLength(500).WithMessage("圖片路徑不得超過 500 字元");

        RuleForEach(x => x.Options).SetValidator(new UpdateServiceItemOptionValidator());
    }
}

public class UpdateServiceItemOptionValidator : AbstractValidator<UpdateServiceItemOptionRequest>
{
    public UpdateServiceItemOptionValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("選項標題為必填")
            .MaximumLength(200).WithMessage("選項標題不得超過 200 字元");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("價格不得小於 0");

        RuleFor(x => x.PriceUnit)
            .MaximumLength(50).WithMessage("價格單位不得超過 50 字元");

        RuleFor(x => x.SubTitle)
            .MaximumLength(200).WithMessage("副標題不得超過 200 字元");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("說明不得超過 1000 字元");
    }
}
