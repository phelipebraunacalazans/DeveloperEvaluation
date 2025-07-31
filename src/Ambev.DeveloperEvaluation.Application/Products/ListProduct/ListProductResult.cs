namespace Ambev.DeveloperEvaluation.Application.Products.ListProduct;

public class ListProductResult
{
    /// <summary>
    /// The list of products.
    /// </summary>
    public ICollection<ProductResult> Products { get; set; } = [];
}