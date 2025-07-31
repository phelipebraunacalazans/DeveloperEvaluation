using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public class GetProductCommand : IRequest<GetProductResult>
{
    /// <summary>
    /// The unique identifier of the product to retrieve.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="GetProductCommand"/>.
    /// </summary>
    /// <param name="id">The ID of the product to retrieve</param>
    public GetProductCommand(Guid id)
    {
        Id = id;
    }
}
