using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProduct;

public class ListProductCommand : IRequest<ListProductResult>
{
    /// <summary>
    /// Initializes a new instance of <see cref="ListProductCommand"/>.
    /// </summary>
    public ListProductCommand()
    {
    }
}