using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(product => product.Name)
            .NotEmpty()
            .MinimumLength(3).WithMessage("Product name Minimum 3 characters long.")
            .MaximumLength(200).WithMessage("Product name Maximum 200 characters.");

        RuleFor(product => product.Price)
            .GreaterThan(0).WithMessage("Product price must be greater than 0.");
    }
}