using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.DeleteCart;

public class DeleteCartProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for DeleteCart feature.
    /// </summary>
    public DeleteCartProfile()
    {
        CreateMap<DeleteProductRequest, DeleteProductCommand>();
    }
}
