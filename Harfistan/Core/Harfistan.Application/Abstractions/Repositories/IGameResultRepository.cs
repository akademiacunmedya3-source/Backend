using Harfistan.Domain.Entities;

namespace Harfistan.Application.Abstractions.Repositories;

public interface IGameResultRepository : IRepository<GameResult>
{
    Task<GameResult?> GetByUserAndDailyWordAsync(Guid userId,int dailyWordId, CancellationToken cancellationToken = default);
    Task<bool> HasUserPlayedTodayAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<GameResult?> AddAsync(GameResult gameResult, CancellationToken cancellationToken = default);
    Task<List<GameResult>> GetUserHistoryPagedAsync(Guid userId,int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}