using Ambev.DeveloperEvaluation.Application.Products;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

public class GetProductProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetProduct feature.
    /// </summary>
    public GetProductProfile()
    {
        CreateMap<GetProductRequest, GetProductCommand>();
        CreateMap<ProductResult, ProductResponse>();
    }
}
