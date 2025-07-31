using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProduct;

public class ListProductCommand : IRequest<ListProductResult>
{
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="ListProductCommand"/>.
    /// </summary>
    /// <param name="name">The name of the product to retrieve</param>
    public ListProductCommand(string name)
    {
        Name = name;
    }
}