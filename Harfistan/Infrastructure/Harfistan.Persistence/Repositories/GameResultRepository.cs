using Harfistan.Application.Abstractions.Repositories;
using Harfistan.Domain.Entities;
using Harfistan.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Harfistan.Persistence.Repositories;

public class GameResultRepository(HarfistanDbContext context) : IGameResultRepository
{
    public DbSet<GameResult> Table => context.Set<GameResult>();

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await context.SaveChangesAsync(cancellationToken);

    public async Task<GameResult?> GetByUserAndDailyWordAsync(Guid userId, int dailyWordId,
        CancellationToken cancellationToken = default) =>
        await Table.Include(gr => gr.DailyWord).ThenInclude(dw => dw.Word).FirstOrDefaultAsync(x => x.UserId == userId && x.DailyWordId == dailyWordId, cancellationToken);

    public async Task<bool> HasUserPlayedTodayAsync(Guid userId, CancellationToken cancellationToken = default) =>
        await Table.Include(gr => gr.DailyWord).AnyAsync(x => x.UserId == userId && x.PlayedAt == DateTime.Today, cancellationToken);

    public async Task<GameResult?> AddAsync(GameResult gameResult, CancellationToken cancellationToken = default) =>
        Table.AddAsync(gameResult).Result.Entity;

    public async Task<(List<GameResult> Games, int TotalCount)> GetUserHistoryPagedAsync(Guid userId, int pageNumber,
        int pageSize, CancellationToken cancellationToken = default)
    {
        var query = Table
            .Include(gr => gr.DailyWord)
            .ThenInclude(dw => dw.Word)
            .Where(gr => gr.UserId == userId)
            .OrderByDescending(gr => gr.PlayedAt);
        
        var totalCount = await query.CountAsync(cancellationToken);

        var games = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        
        return (games, totalCount);
    }
}