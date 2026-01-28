namespace Harfistan.Domain.Entities;

public sealed class UserSession
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? DeviceType { get; set; }
    public string? DeviceModel { get; set; }
    public DateTime LoginAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastActivityAt { get; set; }
    public DateTime? LogoutAt { get; set; }
    public bool IsActive { get; set; } = true;
}