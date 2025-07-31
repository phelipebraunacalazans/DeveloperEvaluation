using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;

public class DeleteProductProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for DeleteProduct feature.
    /// </summary>
    public DeleteProductProfile()
    {
        CreateMap<DeleteProductRequest, DeleteProductCommand>();
    }
}