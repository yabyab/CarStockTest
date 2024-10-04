using System;
using FluentValidation;
using WebApplication1.Models;

namespace WebApplication1.Validators;

public class DealerValidator:AbstractValidator<Dealer>
{
    public DealerValidator(){
        RuleFor(d => d.dealername).NotEmpty().MaximumLength(50);
        RuleFor(d => d.dealeremail).NotEmpty().MaximumLength(50).EmailAddress();
        RuleFor(d => d.dealerid).NotEmpty().GreaterThanOrEqualTo(0);
    }
}
