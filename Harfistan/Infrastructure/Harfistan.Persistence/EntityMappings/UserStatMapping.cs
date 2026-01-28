using Harfistan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harfistan.Persistence.EntityMappings;

public class UserStatMapping : IEntityTypeConfiguration<UserStat>
{
    public void Configure(EntityTypeBuilder<UserStat> builder)
    {
        builder.ToTable("UserStats");
        builder.HasKey(us => us.UserId);
        
        builder.Property(us => us.GuessDistribution)
            .IsRequired()
            .HasDefaultValue("[0,0,0,0,0,0]");

        builder.Property(us => us.WinRate).HasPrecision(5, 2);
        builder.Property(us => us.AverageAttempts).HasPrecision(4, 2);

        builder.HasIndex(us => us.GamesPlayed);
        builder.HasIndex(us => us.CurrentStreak);
    }
}