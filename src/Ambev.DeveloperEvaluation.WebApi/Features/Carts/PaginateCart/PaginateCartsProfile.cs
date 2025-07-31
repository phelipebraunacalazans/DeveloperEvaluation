using Ambev.DeveloperEvaluation.Application.Carts;
using Ambev.DeveloperEvaluation.Application.Carts.PaginatedCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.PaginateCart;

public class PaginateCartsProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for PaginateCarts feature.
    /// </summary>
    public PaginateCartsProfile()
    {
        CreateMap<PaginateCartsRequest, PaginateCartsCommand>();
        CreateMap<CartResult, CartResponse>();
        CreateMap<CartItemResult, CartItemResponse>();
    }
}
