namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

public class UpdateProductRequest
{
    /// <summary>
    /// The unique identifier of the product to update.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets the product's title. Must be unique and contain only valid characters.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Gets the product's full price.
    /// </summary>
    public decimal Price { get; set; }
    
    public int StockQuantity { get; set; }
}