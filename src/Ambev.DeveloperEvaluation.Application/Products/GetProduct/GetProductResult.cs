namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public class GetProductResult
{
    /// <summary>
    /// The unique identifier of the product.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The product's name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The ´product's price.
    /// </summary>
    public double Price { get; set; }
}