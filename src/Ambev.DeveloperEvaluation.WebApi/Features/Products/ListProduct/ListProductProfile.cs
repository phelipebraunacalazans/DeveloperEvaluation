using Ambev.DeveloperEvaluation.Application.Products;
using Ambev.DeveloperEvaluation.Application.Products.ListProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProduct;

public class ListProductProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for ListProduct feature.
    /// </summary>
    public ListProductProfile()
    {
        CreateMap<ListProductRequest, ListProductCommand>();
        CreateMap<ProductResult, ProductResponse>();
    }
}
