namespace Ambev.DeveloperEvaluation.Common.Repositories.Pagination;

public class PaginationQueryResult<T>
{
    /// <summary>
    /// Items of pagination.
    /// </summary>
    public ICollection<T> Items { get; init; } = [];

    /// <summary>
    /// Total items of pagination.
    /// </summary>
    public int TotalItems { get; init; }
}