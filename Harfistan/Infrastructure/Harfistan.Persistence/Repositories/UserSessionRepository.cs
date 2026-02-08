using Harfistan.Application.Abstractions.Repositories;
using Harfistan.Domain.Entities;
using Harfistan.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Harfistan.Persistence.Repositories;

public class UserSessionRepository(HarfistanDbContext _context) : IUserSessionRepository
{
    public DbSet<UserSession> Table => _context.Set<UserSession>();

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);

    public async Task<UserSession> AddAsync(UserSession session, CancellationToken cancellationToken = default) =>
        Table.AddAsync(session, cancellationToken).Result.Entity;

    public async Task<List<UserSession>> GetActiveSessionsAsync(Guid userId,
        CancellationToken cancellationToken = default) =>
        await Table.Where(x => x.UserId == userId && x.IsActive).OrderByDescending(s => s.LoginAt).ToListAsync(cancellationToken);

    public async Task<UserSession?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await Table.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public async Task UpdateAsync(UserSession session, CancellationToken cancellationToken = default) =>
        Table.Update(session);
}