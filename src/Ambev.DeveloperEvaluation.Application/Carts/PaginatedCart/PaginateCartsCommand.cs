using Ambev.DeveloperEvaluation.Common.Repositories.Pagination;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.PaginatedCart;

public class PaginateCartsCommand : PaginationQuery, IRequest<PaginateCartsResult>
{
    
}