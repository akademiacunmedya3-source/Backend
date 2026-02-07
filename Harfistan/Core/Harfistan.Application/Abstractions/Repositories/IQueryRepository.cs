namespace Harfistan.Application.Abstractions.Repositories;

public interface IQueryRepository<T, TId> : IRepository<T> where T: class
{
    IQueryable<T> GetAll(CancellationToken cancellationToken = default);
    Task<T> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
}