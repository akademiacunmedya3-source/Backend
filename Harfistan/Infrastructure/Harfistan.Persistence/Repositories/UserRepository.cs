using Harfistan.Application.Abstractions.Repositories;
using Harfistan.Domain.Entities;
using Harfistan.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Harfistan.Persistence.Repositories;

public class UserRepository(HarfistanDbContext _context) : ICommandRepository<User, Guid>, IQueryRepository<User, Guid>
{
    public DbSet<User> Table => _context.Set<User>();
    public async Task<User> AddAsync(User entity, CancellationToken cancellationToken = default)=> Table.AddAsync(entity, cancellationToken).Result.Entity;
    public async Task<User> RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        User entity = await Table.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        Table.Remove(entity);
        return entity;
    }
    public User Update(User entity, CancellationToken cancellationToken = default) => Table.Update(entity).Entity;
    public async Task<int> SaveAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);
    public IQueryable<User> GetAll(CancellationToken cancellationToken = default) => Table.AsQueryable();
    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => await Table.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
}