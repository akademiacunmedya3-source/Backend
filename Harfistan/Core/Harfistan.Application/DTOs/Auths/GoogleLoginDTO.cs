namespace Harfistan.Application.DTOs.Auths;

public record GoogleLoginDTO
{
    public Guid UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string? AvatarUrl { get; init; }
    public bool IsNewUser { get; init; }
}