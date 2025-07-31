using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.PaginatedCart;

public class PaginateCartsHandler : IRequestHandler<PaginateCartsCommand, PaginateCartsResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of <see cref="PaginateCartsHandler"/>.
    /// </summary>
    /// <param name="cartRepository">The cart repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public PaginateCartsHandler(
        ICartRepository cartRepository,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the PaginateCartsCommand request
    /// </summary>
    /// <param name="request">The PaginateCarts command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The product details if found</returns>
    public async Task<PaginateCartsResult> Handle(PaginateCartsCommand request, CancellationToken cancellationToken)
    {
        var paginationResult = await _cartRepository.PaginateAsync(request, cancellationToken);
        return _mapper.Map<PaginateCartsResult>(paginationResult);
    }
}
