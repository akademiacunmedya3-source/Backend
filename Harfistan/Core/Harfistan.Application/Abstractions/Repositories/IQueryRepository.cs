namespace Harfistan.Application.Abstractions.Repositories;

public interface IQueryRepository<T> : IRepository<T> where T: class
{
    IQueryable<T> GetAll();
    Task<T> GetByIdAsync(int id);
}