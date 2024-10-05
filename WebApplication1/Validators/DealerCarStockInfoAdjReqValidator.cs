using FluentValidation;
using WebApplication1.Models;

namespace WebApplication1.Validators;

public class DealerCarStockInfoAdjReqValidator : AbstractValidator<DealerCarStockInfoAdjRequest>
{
    public DealerCarStockInfoAdjReqValidator()
    {
        RuleFor(req => req.stockid).NotEmpty();
        RuleFor(req => req.make).NotEmpty().MaximumLength(50);
        RuleFor(req => req.model).NotEmpty().MaximumLength(50);
        RuleFor(req => req.year).NotNull().GreaterThanOrEqualTo(1900).LessThanOrEqualTo(DateTime.UtcNow.Year);
    }
}
