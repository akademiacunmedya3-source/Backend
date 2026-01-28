using System.Reflection;
using Harfistan.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Harfistan.Persistence.DbContexts;

public class HarfistanDbContext(DbContextOptions<HarfistanDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserStat> UserStats { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }
    public DbSet<Word> Words { get; set; }
    public DbSet<DailyWord> DailyWords { get; set; }
    public DbSet<GameResult> GameResults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}