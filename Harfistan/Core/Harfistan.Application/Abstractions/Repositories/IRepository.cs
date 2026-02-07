using Microsoft.EntityFrameworkCore;

namespace Harfistan.Application.Abstractions.Repositories;

public interface IRepository<T> where T : class
{
    DbSet<T> Table { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}