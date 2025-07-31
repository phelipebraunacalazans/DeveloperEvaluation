using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public class UpdateProductCommand : IRequest<ProductResult>
{
    /// <summary>
    /// The unique identifier of the product to retrieve
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the name of the product to be updated.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the price for the product.
    /// </summary>
    public decimal Price { get; set; }
    
    public int StockQuantity { get; set; }

    public ValidationResultDetail Validate()
    {
        var validator = new UpdateProductValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}
