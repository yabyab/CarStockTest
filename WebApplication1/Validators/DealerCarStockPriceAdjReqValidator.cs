using System;
using FluentValidation;
using WebApplication1.Models;

namespace WebApplication1.Validators;

public class DealerCarStockPriceAdjReqValidator: AbstractValidator<DealerCarStockPriceAdjRequest>
{
    public DealerCarStockPriceAdjReqValidator(){
        RuleFor(req => req.stockid).NotEmpty();
        RuleFor(req => req.price).NotNull().GreaterThanOrEqualTo(0).LessThanOrEqualTo((decimal)9999999.99);
    }
}
