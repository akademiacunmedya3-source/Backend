namespace Harfistan.Application.Abstractions.Repositories;

public interface ICommandRepository<T> : IRepository<T> where T: class
{
    Task<T> AddAsync(T entity);
    Task<T> RemoveAsync(int id);
    T Update(T entity);
    Task<int> SaveAsync();
}