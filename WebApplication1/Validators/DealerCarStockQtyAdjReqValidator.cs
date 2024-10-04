using System;
using FluentValidation;
using WebApplication1.Models;

namespace WebApplication1.Validators;

public class DealerCarStockQtyAdjReqValidator : AbstractValidator<DealerCarStockQtyAdjRequest>
{
    public DealerCarStockQtyAdjReqValidator(){
        RuleFor(req => req.stockid).NotEmpty();
        RuleFor(req => req.quantity).NotNull().GreaterThanOrEqualTo(0).LessThanOrEqualTo(999999999);
    }
}
