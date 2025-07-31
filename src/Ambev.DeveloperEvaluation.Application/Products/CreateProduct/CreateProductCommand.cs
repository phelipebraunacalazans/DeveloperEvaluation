using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Command for creating a new product.
/// </summary>
public class CreateProductCommand : IRequest<ProductResult>
{
    /// <summary>
    /// Gets or sets the name of the product to be created.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the price for the product.
    /// </summary>
    public double Price { get; set; }
    
    public int StockQuantity { get; set; }

    public ValidationResultDetail Validate()
    {
        var validator = new CreateProductValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}
