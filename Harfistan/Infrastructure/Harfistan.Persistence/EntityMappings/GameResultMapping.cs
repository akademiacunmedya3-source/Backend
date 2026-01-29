using Harfistan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harfistan.Persistence.EntityMappings;

public class GameResultMapping : IEntityTypeConfiguration<GameResult>
{
    public void Configure(EntityTypeBuilder<GameResult> builder)
    {
        builder.ToTable("GameResults");
        builder.HasKey(gr => gr.Id);

        builder.Property(gr => gr.GuessesJson).IsRequired().HasDefaultValue("[]");
        builder.Property(gr => gr.DeviceType).HasMaxLength(20);

        builder.HasIndex(gr => new { gr.UserId, gr.DailyWordId }).IsUnique();
        builder.HasIndex(gr => gr.PlayedAt);
        builder.HasIndex(gr => gr.IsWin);
        
        builder.HasOne(gr => gr.User)
            .WithMany(u => u.GameResults)
            .HasForeignKey(gr => gr.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(gr => gr.DailyWord)
            .WithMany(dw => dw.GameResults)
            .HasForeignKey(gr => gr.DailyWordId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
