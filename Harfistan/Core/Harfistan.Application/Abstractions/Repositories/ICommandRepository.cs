namespace Harfistan.Application.Abstractions.Repositories;

public interface ICommandRepository<T> : IRepository<T> where T: class
{
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> RemoveAsync(Guid id, CancellationToken cancellationToken = default);
    T Update(T entity, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(CancellationToken cancellationToken = default);
}