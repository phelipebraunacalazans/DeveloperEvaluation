using Ambev.DeveloperEvaluation.Common.Repositories.Pagination;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProduct;

public class ListProductResult : PaginationQueryResult<ProductResult>
{
    /// <summary>
    /// The list of products.
    /// </summary>
    public ICollection<ProductResult> Products { get; set; } = [];
}