using Ambev.DeveloperEvaluation.Application.Carts;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;

public class UpdateCartProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateCart feature.
    /// </summary>
    public UpdateCartProfile()
    {
        CreateMap<UpdateCartRequest, UpdateCartCommand>();
        CreateMap<CartResult, CartResponse>();
        CreateMap<UpdateCartItemRequest, UpdateCartItem>();
    }
}
