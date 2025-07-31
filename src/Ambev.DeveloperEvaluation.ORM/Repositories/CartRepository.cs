using Ambev.DeveloperEvaluation.Common.Repositories.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.ExtensionMethods;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class CartRepository(DefaultContext context) : ICartRepository
{
    public async Task<Cart> CreateAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        await context.Carts.AddAsync(cart, cancellationToken);
        return cart;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cart = await GetByIdAsync(id, cancellationToken);
        if (cart == null)
            return false;

        context.Carts.Remove(cart);
        return true;
    }

    public async Task<Cart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Carts.FirstOrDefaultAsync(o=> o.Id == id, cancellationToken);

    public async Task<PaginationQueryResult<Cart>> PaginateAsync(PaginationQuery paging, CancellationToken cancellationToken = default)
        => await context.Carts.ToPaginateAsync(paging, cancellationToken);
}