using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

public class DeleteCartCommand : IRequest<DeleteCartResponse>
{
    /// <summary>
    /// The unique identifier of the cart to delete.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="DeleteCartResponse"/>.
    /// </summary>
    /// <param name="id">The ID of the cart to delete</param>
    public DeleteCartCommand(Guid id)
    {
        Id = id;
    }
}
