namespace Harfistan.Application.Abstractions.Repositories;

public interface IQueryRepository<T> : IRepository<T> where T: class
{
    IQueryable<T> GetAll(CancellationToken cancellationToken = default);
    Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}