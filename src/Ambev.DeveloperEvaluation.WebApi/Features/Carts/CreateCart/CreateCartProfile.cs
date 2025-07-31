using Ambev.DeveloperEvaluation.Application.Carts;
using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

public class CreateCartProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateCart feature.
    /// </summary>
    public CreateCartProfile()
    {
        CreateMap<CreateCartRequest, CreateCartCommand>();
        CreateMap<CartResult, CartResponse>();
        CreateMap<CreateCartItemRequest, CreateCartItem>();
    }
}
