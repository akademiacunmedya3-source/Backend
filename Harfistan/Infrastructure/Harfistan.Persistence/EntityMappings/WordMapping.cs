using Harfistan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harfistan.Persistence.EntityMappings;

public class WordMapping : IEntityTypeConfiguration<Word>
{
    public void Configure(EntityTypeBuilder<Word> builder)
    {
        builder.ToTable("Words");
        builder.HasKey(w => w.Id);
        builder.Property(w => w.Text).HasMaxLength(20);
        builder.Property(w => w.Category).HasMaxLength(50);
        builder.Property(w => w.AverageSuccessRate).HasPrecision(5, 2);
        
        builder.HasIndex(w => w.Text).IsUnique();
        builder.HasIndex(w => new { w.Length, w.IsCommon });
        builder.HasIndex(w => w.DifficultyLevel);
    }
}