using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProduct;

public class ListProductHandler : IRequestHandler<ListProductCommand, ListProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of <see cref="ListProductHandler"/>.
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public ListProductHandler(
        IProductRepository productRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the ListProductCommand request
    /// </summary>
    /// <param name="request">The ListProduct command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The product details if found</returns>
    public async Task<ListProductResult> Handle(ListProductCommand request, CancellationToken cancellationToken)
        => _mapper.Map<ListProductResult>(await _productRepository.PaginateAsync(request, cancellationToken));
}
