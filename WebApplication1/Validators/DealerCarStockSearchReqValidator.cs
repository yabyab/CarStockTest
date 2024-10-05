using FluentValidation;
using WebApplication1.Models;

namespace WebApplication1.Validators;

public class DealerCarStockSearchReqValidator :  AbstractValidator<DealerCarStockInfoAdjRequest>
{
    public DealerCarStockSearchReqValidator()
    {
        RuleFor(req => req.make).MaximumLength(50);
        RuleFor(req => req.model).MaximumLength(50);
        RuleFor(req => req.year).GreaterThanOrEqualTo(1900).LessThanOrEqualTo(DateTime.UtcNow.Year);
    }
}
