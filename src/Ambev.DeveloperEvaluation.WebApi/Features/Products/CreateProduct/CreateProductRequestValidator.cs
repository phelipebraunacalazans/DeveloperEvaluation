using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProductRequestValidator"/> with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// <listheader>The validation includes checking:</listheader>
    /// <list type="bullet">Name: Required, must be between 3 and 100 characters</list>
    /// <list type="bullet">Price: Must be greater than zero</list>
    /// </remarks>
    public CreateProductRequestValidator()
    {
        RuleFor(product => product.Name).NotEmpty().Length(3, 100);
        RuleFor(product => product.Price).GreaterThan(0);
    }
}
