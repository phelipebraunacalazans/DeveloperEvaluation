using Ambev.DeveloperEvaluation.Common.Repositories.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.PaginatedCart;

public class PaginateCartsProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for ListProduct operation.
    /// </summary>
    public PaginateCartsProfile()
    {
        CreateMap<PaginationQueryResult<Cart>, PaginateCartsResult>();
        CreateMap<Cart, CartResult>();
        CreateMap<CartItem, CartItemResult>();
    }
}
