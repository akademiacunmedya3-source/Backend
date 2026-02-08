using Harfistan.Application.Abstractions.Repositories;
using Harfistan.Application.Features.LeaderBoards.Queries.GetLeaderBoard;
using Harfistan.Domain.Entities;
using Harfistan.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Harfistan.Persistence.Repositories;

public class UserRepository(HarfistanDbContext _context) : IUserRepository
{
    public DbSet<User> Table => _context.Set<User>();

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await Table.Include(u => u.Stats).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<User?> GetByDeviceIdAsync(string deviceId, CancellationToken cancellationToken = default) =>
        await Table.Include(u => u.Stats).FirstOrDefaultAsync(x => x.DeviceId == deviceId, cancellationToken);

    public async Task<User?> GetByGoogleIdAsync(string googleId, CancellationToken cancellationToken = default) =>
        await Table.Include(u => u.Stats).FirstOrDefaultAsync(x => x.GoogleId == googleId, cancellationToken);

    public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default) =>
        Table.AddAsync(user, cancellationToken).Result.Entity;

    public async Task<List<User>> GetTopPlayersAsync(int count, LeaderboardType leaderboardType,
        CancellationToken cancellationToken = default)
    {
        var query = Table.Include(u => u.Stats).Where(u => u.Stats != null && u.Stats.GamesPlayed > 0);
        
        query = leaderboardType switch
        {
            LeaderboardType.WinRate => query
                .Where(u => u.Stats.GamesPlayed >= 5)
                .OrderByDescending(u => u.Stats.WinRate)
                .ThenByDescending(u => u.Stats.GamesWon),
            LeaderboardType.TotalWins => query
                .OrderByDescending(u => u.Stats.GamesWon)
                .ThenByDescending(u => u.Stats.WinRate),
            LeaderboardType.CurrentStreak => query
                .OrderByDescending(u => u.Stats.CurrentStreak)
                .ThenByDescending(u => u.Stats.MaxStreak),
            LeaderboardType.MaxStreak => query
                .OrderByDescending(u=> u.Stats.MaxStreak)
                .ThenByDescending(u => u.Stats.CurrentStreak),
            _ => query.OrderByDescending(u=> u.Stats.WinRate)
        };

        return await query.Take(count).ToListAsync(cancellationToken);
    }

    public async Task<int> GetUserRankAsync(Guid userId, LeaderboardType leaderboardType,
        CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(userId, cancellationToken);
        if (user?.Stats == null) return 0;
        
        var query = Table
            .Include(u => u.Stats)
            .Where(u => u.Stats != null && u.Stats.GamesPlayed > 0);

        int rank = leaderboardType switch
        {
            LeaderboardType.WinRate => await query
                .Where(u => u.Stats!.GamesPlayed >= 5)
                .CountAsync(u => 
                        u.Stats!.WinRate > user.Stats.WinRate ||
                        (u.Stats!.WinRate == user.Stats.WinRate && u.Stats!.GamesWon > user.Stats.GamesWon),
                    cancellationToken),
            
            LeaderboardType.TotalWins => await query
                .CountAsync(u => 
                        u.Stats!.GamesWon > user.Stats.GamesWon ||
                        (u.Stats!.GamesWon == user.Stats.GamesWon && u.Stats!.WinRate > user.Stats.WinRate),
                    cancellationToken),
            
            LeaderboardType.CurrentStreak => await query
                .CountAsync(u => 
                        u.Stats!.CurrentStreak > user.Stats.CurrentStreak ||
                        (u.Stats!.CurrentStreak == user.Stats.CurrentStreak && u.Stats!.MaxStreak > user.Stats.MaxStreak),
                    cancellationToken),
            
            LeaderboardType.MaxStreak => await query
                .CountAsync(u => 
                        u.Stats!.MaxStreak > user.Stats.MaxStreak ||
                        (u.Stats!.MaxStreak == user.Stats.MaxStreak && u.Stats!.CurrentStreak > user.Stats.CurrentStreak),
                    cancellationToken),
            
            _ => 0
        };
        return rank + 1;
    }

    public async Task<int> GetTotalPlayersCountAsync(CancellationToken cancellationToken = default)
    {
        return await Table.Include(u => u.Stats).CountAsync(u => u.Stats != null && u.Stats.GamesPlayed > 0, cancellationToken);
    }
}