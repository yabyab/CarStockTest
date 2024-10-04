using System;
using FluentValidation;
using WebApplication1.Models;

namespace WebApplication1.Validators;

public class JwtAuthRequestValidator:AbstractValidator<JwtAuthRequest>
{
    public JwtAuthRequestValidator(){
        RuleFor(req => req.dealername).NotEmpty().MaximumLength(50);
        RuleFor(req => req.dealeremail).NotEmpty().MaximumLength(50).EmailAddress();
    }
}
