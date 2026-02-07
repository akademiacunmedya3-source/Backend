using Harfistan.Application.Abstractions.Repositories;
using Harfistan.Domain.Entities;
using Harfistan.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Harfistan.Persistence.Repositories;

public class DailyWordRepository(HarfistanDbContext context) : IDailyWordRepository
{
    public DbSet<DailyWord> Table => context.Set<DailyWord>();
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await context.SaveChangesAsync(cancellationToken);
    public async Task<DailyWord?> GetByDateAsync(DateTime date, CancellationToken cancellationToken = default) => await Table.FirstOrDefaultAsync(x => x.Date == date, cancellationToken);
    public async Task<DailyWord?> GetTodayAsync(CancellationToken cancellationToken = default) => await GetByDateAsync(DateTime.Today, cancellationToken);
    public async Task<DailyWord> AddAsync(DailyWord dailyWord, CancellationToken cancellationToken = default) => Table.AddAsync(dailyWord, cancellationToken).Result.Entity;
    public async Task UpdateAsync(DailyWord dailyWord, CancellationToken cancellationToken = default) =>
        Table.Update(dailyWord);
    public async Task<bool> ExistsForDateAsync(DateTime date, CancellationToken cancellationToken = default) => await Table.AnyAsync(x => x.Date == date, cancellationToken);
}