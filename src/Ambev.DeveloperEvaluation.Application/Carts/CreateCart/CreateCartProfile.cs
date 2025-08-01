using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

public class CreateCartProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateProduct operation
    /// </summary>
    public CreateCartProfile()
    {
        CreateMap<Cart, CartResult>()
            .ForMember(x => x.UserId, mce => mce.MapFrom(x => x.BoughtById))
            .ForMember(x => x.Date, mce => mce.MapFrom(x => x.SoldAt))
            .ForMember(x => x.Branch, mce => mce.MapFrom(x => x.StoreName))
            .ForMember(x => x.IsCancelled, mce => mce.MapFrom((c, r) => c.PurchaseStatus is PurchaseStatus.Cancelled))
            .ForMember(x => x.Products, mce => mce.MapFrom(x => x.Items));

        CreateMap<CartItem, CartItemResult>();
    }
}
