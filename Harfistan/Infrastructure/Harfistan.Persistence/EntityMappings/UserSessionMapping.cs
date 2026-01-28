using Harfistan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harfistan.Persistence.EntityMappings;

public class UserSessionMapping : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable("UserSessions");
        builder.HasKey(us => us.Id);

        builder.Property(us => us.IpAddress).HasMaxLength(45);
        builder.Property(us => us.UserAgent).HasMaxLength(500);
        builder.Property(us => us.DeviceType).HasMaxLength(20);
        builder.Property(us => us.DeviceModel).HasMaxLength(100);
        
        builder.HasIndex(us => us.UserId);
        builder.HasIndex(us => us.LoginAt);
        builder.HasIndex(us => us.IsActive);
        
        builder.HasOne(us => us.User)
            .WithMany(u => u.Sessions)
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}