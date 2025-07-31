namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

public class CreateProductRequest
{
    /// <summary>
    /// Gets the product's title. Must be unique and contain only valid characters.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Gets the product's full price.
    /// </summary>
    public decimal Price { get; set; }
}