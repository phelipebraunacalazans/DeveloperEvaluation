using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProduct;

public class ListProductProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for ListProduct operation.
    /// </summary>
    public ListProductProfile()
    {
        CreateMap<Product, ProductResult>();
    }
}
