using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    /// <summary>
    /// Initializes a new instance of the UpdateProductCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Id: Required
    /// - Productname: Required, must be between 3 and 100 characters
    /// - ProductPrice: Must be greater than zero
    /// </remarks>
    public UpdateProductValidator()
    {
        RuleFor(product => product.Id).NotEmpty();
        RuleFor(product => product.Name).NotEmpty().Length(3, 50);
        RuleFor(product => product.Price).GreaterThan(0);
    }
}