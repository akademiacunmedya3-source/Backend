using System.Security.Cryptography;

namespace Harfistan.Domain.Entities;

public sealed class User
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? GoogleId { get; set; }
    public string? DeviceId { get; set; }
    public string AuthProvider { get; set; } = "Anonymous";
    public string? DisplayName { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public UserStat? Stats { get; set; }
    public List<UserSession> Sessions { get; set; } = new();
    public List<GameResult> GameResults { get; set; } = new();
}