using System;
using FluentValidation;
using WebApplication1.Models;

namespace WebApplication1.Validators;

public class DealerCarStockValidator: AbstractValidator<DealerCarStock>
{
    public DealerCarStockValidator(){
        RuleFor(dcs => dcs.stockid).NotNull().GreaterThanOrEqualTo(0);
        RuleFor(dcs => dcs.dealerid).NotEmpty();
        RuleFor(dcs => dcs.make).NotEmpty().MaximumLength(50);
        RuleFor(dcs => dcs.model).NotEmpty().MaximumLength(50);
        RuleFor(dcs => dcs.year).NotNull().GreaterThanOrEqualTo(1900).LessThanOrEqualTo(DateTime.UtcNow.Year);
        RuleFor(dcs => dcs.price).NotNull().GreaterThanOrEqualTo(0).LessThanOrEqualTo((decimal)9999999.99);
        RuleFor(dcs => dcs.quantity).NotNull().GreaterThanOrEqualTo(0).LessThanOrEqualTo(999999999);
    }

}
