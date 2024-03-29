﻿using BasketTestLib.Models;
using FluentValidation;

namespace BasketTestLib.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
       public ProductValidator()
        {
            RuleFor(x => x.UnitPrice).NotEmpty();
            RuleFor(x => x.UnitPrice).GreaterThan(0.0m);
        }
    }
}
