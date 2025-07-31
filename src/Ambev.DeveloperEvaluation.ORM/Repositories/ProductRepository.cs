using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of ProductRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public ProductRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        return product;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await GetByIdAsync(id, cancellationToken);
        if (product == null)
            return false;

        _context.Products.Remove(product);
        return true;
    }

    /// <inheritdoc/>
    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(u => u.Name == name, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ICollection<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .ToArrayAsync(cancellationToken);
    }
}