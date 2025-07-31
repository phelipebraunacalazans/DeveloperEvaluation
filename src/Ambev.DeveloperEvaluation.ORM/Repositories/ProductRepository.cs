using Ambev.DeveloperEvaluation.Common.Repositories.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.ExtensionMethods;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class ProductRepository(DefaultContext context) : IProductRepository
{
    /// <inheritdoc/>
    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken)
    {
        await context.Products.AddAsync(product, cancellationToken);
        return product;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await GetByIdAsync(id, cancellationToken);
        if (product == null)
            return false;

        context.Products.Remove(product);
        return true;
    }

    /// <inheritdoc/>
    public async Task<ICollection<Product>> ListByIdsAsync(Guid[] ids, CancellationToken cancellationToken = default)
    {
        return await context.Products
            .Where(p => ids.Contains(p.Id))
            .ToArrayAsync(cancellationToken);
    }
    
    /// <inheritdoc/>
    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Products.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

    /// <inheritdoc/>
    public async Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => await context.Products.FirstOrDefaultAsync(u => u.Name == name, cancellationToken);

    /// <inheritdoc/>
    public async Task<PaginationQueryResult<Product>> PaginateAsync(PaginationQuery query, CancellationToken cancellationToken = default)
        => await context.Products.ToPaginateAsync(query, cancellationToken);
}