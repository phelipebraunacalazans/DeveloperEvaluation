namespace Ambev.DeveloperEvaluation.Application.Products;

public class ProductResult
{
    /// <summary>
    /// The unique identifier of the product.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The product's name.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// The product's price.
    /// </summary>
    public decimal Price { get; set; }
}