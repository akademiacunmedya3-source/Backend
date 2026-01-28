using Harfistan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harfistan.Persistence.EntityMappings;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Email).HasMaxLength(100);
        builder.Property(u => u.GoogleId).HasMaxLength(255);
        builder.Property(u => u.DeviceId).HasMaxLength(255);
        builder.Property(u => u.AuthProvider).IsRequired().HasMaxLength(20);
        builder.Property(u => u.DisplayName).HasMaxLength(50);
        builder.Property(u => u.AvatarUrl).HasMaxLength(500);
        
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.DeviceId);
        builder.HasIndex(u => u.GoogleId).IsUnique();
        
        builder.HasOne(u => u.Stats)
            .WithOne(s => s.User)
            .HasForeignKey<UserStat>(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}