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
    /// Deletes a products from the repository
    /// </summary>
    /// <param name="id">The unique identifier of the product to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the product was deleted, false if not found</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves all products that contain the name.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The list of products</returns>
    Task<ICollection<Product>> GetAllAsync(CancellationToken cancellationToken = default);
}