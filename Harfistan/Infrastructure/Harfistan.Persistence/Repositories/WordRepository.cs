using Harfistan.Application.Abstractions.Repositories;
using Harfistan.Domain.Entities;
using Harfistan.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Harfistan.Persistence.Repositories;

public class WordRepository(HarfistanDbContext context) : IWordRepository
{
    public DbSet<Word> Table  => context.Set<Word>();
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await context.SaveChangesAsync(cancellationToken);

    public async Task<Word?> GetByIdAsync(int id, CancellationToken cancellationToken = default) => await Table.FindAsync(id, cancellationToken);

    public async Task<Word?> GetByTextAsync(string text, CancellationToken cancellationToken = default) => await Table.FirstOrDefaultAsync(x => x.Text == text, cancellationToken);

    public async Task<bool> ExistsAsync(string text, CancellationToken cancellationToken = default) => await Table.AnyAsync(x => x.Text == text, cancellationToken);

    public async Task<Word> GetRandomWordAsync(int length, CancellationToken cancellationToken = default) => await Table.OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync(x => x.Length == length, cancellationToken);

    public async Task<List<Word>> GetCommonWordsAsync(int length, CancellationToken cancellationToken = default) => await Table.Where(x => x.IsCommon && x.Length == length).ToListAsync(cancellationToken);
}