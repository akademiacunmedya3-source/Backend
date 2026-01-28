using Harfistan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harfistan.Persistence.EntityMappings;

public class DailyWordMapping : IEntityTypeConfiguration<DailyWord>
{
    public void Configure(EntityTypeBuilder<DailyWord> builder)
    {
        builder.ToTable("DailyWords");
        builder.HasKey(dw => dw.Id);
        
        builder.Property(w => w.Date).HasColumnType("date");
        builder.Property(dw => dw.WordHash).IsRequired().HasMaxLength(64);
        builder.Property(dw => dw.WinRate).HasPrecision(5, 2);
        builder.Property(dw => dw.AverageAttempts).HasPrecision(4, 2);
        
        builder.HasIndex(dw => dw.Date).IsUnique();
        builder.HasIndex(dw => dw.CreatedAt);
        
        builder.HasOne(dw => dw.Word)
            .WithMany(w => w.DailyWords)
            .HasForeignKey(dw => dw.WordId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}