using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Validator for <see cref="CreateProductCommand"/> that defines validation rules for product creation command.
/// </summary>
public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProductValidator"/> with defined validation rules.
    /// </summary>
    public CreateProductValidator()
    {
        RuleFor(product => product.Name).NotEmpty().Length(3, 200);
        RuleFor(product => product.Price).GreaterThan(0);
        RuleFor(product => product.StockQuantity).GreaterThan(0);
    }
}
