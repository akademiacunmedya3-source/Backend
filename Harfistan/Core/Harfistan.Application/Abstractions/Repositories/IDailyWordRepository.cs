using Harfistan.Domain.Entities;

namespace Harfistan.Application.Abstractions.Repositories;

public interface IDailyWordRepository : IRepository<DailyWord>
{
    Task<DailyWord?> GetByDateAsync(DateTime date, CancellationToken cancellationToken = default);
    Task<DailyWord?> GetTodayAsync(CancellationToken cancellationToken = default);
    Task<DailyWord> AddAsync(DailyWord dailyWord, CancellationToken cancellationToken = default);
    Task UpdateAsync(DailyWord dailyWord, CancellationToken cancellationToken = default);
    Task<bool> ExistsForDateAsync(DateTime date, CancellationToken cancellationToken = default);
}