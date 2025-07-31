namespace Ambev.DeveloperEvaluation.Common.Interfaces.Repositories;

public interface IUnitOfWork
{
    Task<int> ApplyChangesAsync(CancellationToken cancellationToken = default);
}