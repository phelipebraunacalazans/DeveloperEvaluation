using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProductRequestValidator"/> with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// <listheader>The validation includes checking:</listheader>
    /// <list type="bullet">Id: Required, must be empty</list>
    /// <list type="bullet">Name: Required, must be between 3 and 100 characters</list>
    /// <list type="bullet">Price: Must be greater than zero</list>
    /// </remarks>
    public UpdateProductRequestValidator()
    {
        RuleFor(product => product.Id).NotEmpty();
        RuleFor(product => product.Name).NotEmpty().Length(3, 100);
        RuleFor(product => product.Price).GreaterThan(0);
    }
}
