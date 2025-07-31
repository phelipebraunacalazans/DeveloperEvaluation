namespace Ambev.DeveloperEvaluation.Common.Repositories.Pagination;

public class PaginationQuery
{
    /// <summary>
    /// Page number of pagination.
    /// </summary>
    public int Page { get; init; }

    /// <summary>
    /// Page size of pagination.
    /// </summary>
    public int Size { get; init; }

    /// <summary>
    /// Ordering of pagination.
    /// </summary>
    public IEnumerable<KeyValuePair<string, SortDirection>> Orders { get; init; } = [];
}