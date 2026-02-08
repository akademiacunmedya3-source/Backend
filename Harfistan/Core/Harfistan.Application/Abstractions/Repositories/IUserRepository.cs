using Harfistan.Application.Features.LeaderBoards.Queries.GetLeaderBoard;
using Harfistan.Domain.Entities;

namespace Harfistan.Application.Abstractions.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByDeviceIdAsync(string deviceId, CancellationToken cancellationToken = default);
    Task<User?> GetByGoogleIdAsync(string googleId, CancellationToken cancellationToken = default);
    Task<User> AddAsync(User user, CancellationToken cancellationToken = default);
    Task<List<User>> GetTopPlayersAsync(int count,LeaderboardType leaderboardType, CancellationToken cancellationToken = default);
    Task<int> GetUserRankAsync(Guid userId, LeaderboardType leaderboardType, CancellationToken cancellationToken = default);
    Task<int> GetTotalPlayersCountAsync(CancellationToken cancellationToken = default);
}