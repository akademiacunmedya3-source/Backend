namespace Harfistan.Application.Abstractions.Repositories;

public interface IQueryRepository<T> : IRepository<T> where T: class
{
    IQueryable<T> GetAll(CancellationToken cancellationToken = default);
    Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}