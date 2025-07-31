using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

public class GetCartCommand : IRequest<CartResult>
{
    /// <summary>
    /// The unique identifier of the cart to retrieve.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="CartResult"/>.
    /// </summary>
    /// <param name="id">The ID of the cart to retrieve</param>
    public GetCartCommand(Guid id)
    {
        Id = id;
    }
}