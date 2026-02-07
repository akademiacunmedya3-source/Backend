using Harfistan.Application.Abstractions.Repositories;
using Harfistan.Domain.Entities;
using Harfistan.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Harfistan.Persistence.Repositories;

public class UserRepository(HarfistanDbContext _context) : IUserRepository
{
    public DbSet<User> Table => _context.Set<User>();
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => await Table.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    public async Task<User?> GetByDeviceIdAsync(string deviceId, CancellationToken cancellationToken = default) => await Table.FirstOrDefaultAsync(x => x.DeviceId == deviceId, cancellationToken);
    public async Task<User?> GetByGoogleIdAsync(string googleId, CancellationToken cancellationToken = default) => await Table.FirstOrDefaultAsync(x => x.GoogleId == googleId, cancellationToken); 
    public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default) => Table.AddAsync(user, cancellationToken).Result.Entity;
}