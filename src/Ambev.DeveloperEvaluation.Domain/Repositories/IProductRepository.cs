using Ambev.DeveloperEvaluation.Common.Repositories.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface IProductRepository
{
    /// <summary>
    /// Creates a new products in the repository
    /// </summary>
    /// <param name="products">The user to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created products</returns>
    Task<Product> CreateAsync(Product user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a products by their unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the products</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The products if found, null otherwise</returns>
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a products by their name
    /// </summary>
    /// <param name="name">The name to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user if found, null otherwise</returns>
    Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieve a list of products by your ids.
    /// </summary>
    /// <param name="ids">List of identifiers of product</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of product contains your ids.</returns>
    Task<ICollection<Product>> ListByIdsAsync(Guid[] ids, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deletes a products from the repository
    /// </summary>
    /// <param name="id">The unique identifier of the product to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the product was deleted, false if not found</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves all paginated products.
    /// </summary>
    /// <param name="query">Query to paginate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The list of paginated products</returns>
    Task<PaginationQueryResult<Product>> PaginateAsync(PaginationQuery query, CancellationToken cancellationToken = default);
}