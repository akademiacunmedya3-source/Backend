using Harfistan.Domain.Entities;

namespace Harfistan.Application.Abstractions.Repositories;

public interface IUserSessionRepository : IRepository<UserSession>
{
    Task<UserSession> AddAsync(UserSession session, CancellationToken cancellationToken = default);
    Task<List<UserSession>> GetActiveSessionsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<UserSession?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task UpdateAsync(UserSession session, CancellationToken cancellationToken = default);
}