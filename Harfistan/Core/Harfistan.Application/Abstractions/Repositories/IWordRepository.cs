using Harfistan.Domain.Entities;

namespace Harfistan.Application.Abstractions.Repositories;

public interface IWordRepository : IRepository<Word>
{
    Task<Word?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Word?> GetByTextAsync(string text, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string text, CancellationToken cancellationToken = default);
    Task<Word> GetRandomWordAsync(int length, CancellationToken cancellationToken = default);
    Task<List<Word>> GetCommonWordsAsync(int length, CancellationToken cancellationToken = default);
}