using Ambev.DeveloperEvaluation.Common.Repositories.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ICartRepository
{
    /// <summary>
    /// Creates a new cart in the repository.
    /// </summary>
    /// <param name="cart">The cart to create</param>
    Task<Cart> CreateAsync(Cart cart, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deletes a cart from the repository.
    /// </summary>
    /// <param name="id">The unique identifier of the cart to delete</param>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves a cart by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the cart</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cart if found, null otherwise</returns>
    Task<Cart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves all paginated carts.
    /// </summary>
    /// <param name="paging">Info to paginate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The list of paginated carts</returns>
    Task<PaginationQueryResult<Cart>> PaginateAsync(PaginationQuery paging, CancellationToken cancellationToken = default);
}