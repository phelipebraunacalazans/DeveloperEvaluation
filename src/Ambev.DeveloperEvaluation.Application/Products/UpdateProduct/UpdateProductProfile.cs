using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public class UpdateProductProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateProduct operation
    /// </summary>
    public UpdateProductProfile()
    {
        CreateMap<Product, ProductResult>();
    }
}
