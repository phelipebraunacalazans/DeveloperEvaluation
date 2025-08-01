using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

public class GetCartHandler : IRequestHandler<GetCartCommand, CartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of <see cref="GetCartHandler"/>.
    /// </summary>
    /// <param name="cartRepository">The cart repository</param>
    /// <param name="unitOfWork">Unit of work</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetCartHandler(
        ICartRepository cartRepository,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the <see cref="GetCartCommand"/> request.
    /// </summary>
    /// <param name="request">The GetCart command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The product details if found</returns>
    public async Task<CartResult> Handle(GetCartCommand request, CancellationToken cancellationToken)
    {
        var validator = new GetCartValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var cart = await _cartRepository.GetByIdAsync(request.Id, cancellationToken);
        if (cart is null)
        {
            throw new ValidationException(
            [
                new(string.Empty, $"Cart with ID {request.Id} was not found."),
            ]);
        }

        return _mapper.Map<CartResult>(cart);
    }
}
