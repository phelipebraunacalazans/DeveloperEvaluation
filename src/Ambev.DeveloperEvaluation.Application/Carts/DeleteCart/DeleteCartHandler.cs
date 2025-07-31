
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Common.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

public class DeleteCartHandler : IRequestHandler<DeleteCartCommand, DeleteCartResponse>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of <see cref="DeleteCartHandler"/>.
    /// </summary>
    /// <param name="cartRepository">The cart repository</param>
    /// <param name="unitOfWork">Unit of work</param>
    public DeleteCartHandler(
        ICartRepository cartRepository,
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles the <see cref="DeleteProductCommand"/> request.
    /// </summary>
    /// <param name="request">The DeleteProduct command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the delete operation</returns>
    public async Task<DeleteCartResponse> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {
        var validator = new DeleteCartValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        await _cartRepository.DeleteAsync(request.Id);

        var success = (await _unitOfWork.ApplyChangesAsync(cancellationToken)) > 0;

        if (!success)
        {
            throw new ValidationException(
            [
                new(string.Empty, $"Cart with ID {request.Id} was not found."),
            ]);
        }

        return new DeleteCartResponse { Success = true };
    }
}
