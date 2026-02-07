using Harfistan.Application.Abstractions.Repositories;
using Harfistan.Domain.Entities;
using Harfistan.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Harfistan.Persistence.Repositories;

public class UserSessionRepository(HarfistanDbContext _context) : ICommandRepository<UserSession, int>, IQueryRepository<UserSession, int>
{
    public DbSet<UserSession> Table => _context.Set<UserSession>();
    public async Task<UserSession> AddAsync(UserSession entity, CancellationToken cancellationToken = default)=> Table.AddAsync(entity, cancellationToken).Result.Entity;
    public async Task<UserSession> RemoveAsync(int id, CancellationToken cancellationToken = default)
    {
        UserSession entity = await Table.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        Table.Remove(entity);
        return entity;
    }
    public UserSession Update(UserSession entity, CancellationToken cancellationToken = default) => Table.Update(entity).Entity;
    public async Task<int> SaveAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);
    public IQueryable<UserSession> GetAll(CancellationToken cancellationToken = default) => Table.AsQueryable();
    public async Task<UserSession> GetByIdAsync(int id, CancellationToken cancellationToken = default) => await Table.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
}