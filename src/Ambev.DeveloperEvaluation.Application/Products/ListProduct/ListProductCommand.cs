using Ambev.DeveloperEvaluation.Common.Repositories.Pagination;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProduct;

public class ListProductCommand(string name) : PaginationQuery, IRequest<ListProductResult>
{
    public string Name { get; } = name;
}