using Harfistan.Application.Abstractions.Repositories;
using Harfistan.Application.DTOs.Auths;
using Harfistan.Domain.Entities;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Harfistan.Application.Features.Auths.Commands.LoginWithGoogle;

public record LoginWithGoogleCommand : IRequest<GoogleLoginDTO>
{
    public string GoogleId { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? DisplayName { get; init; }
    public string? AvatarUrl { get; init; }
    public string? DeviceType { get; init; }
    public string? IpAddress { get; init; }
    public string? UserAgent { get; init; }
}

public class LoginWithGoogleCommandHandler(
    IUserRepository userRepository,
    IUserSessionRepository userSessionRepository) : IRequestHandler<LoginWithGoogleCommand, GoogleLoginDTO>
{
    public async ValueTask<GoogleLoginDTO> Handle(LoginWithGoogleCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.GetByGoogleIdAsync(request.GoogleId, cancellationToken);
        if (existingUser is not null)
        {
            var session = new UserSession
            {
                UserId = existingUser.Id,
                LoginAt = DateTime.UtcNow,
                IsActive = true,
                DeviceType = request.DeviceType,
                IpAddress = request.IpAddress,
                UserAgent = request.UserAgent
            };

            await userSessionRepository.AddAsync(session, cancellationToken);
            await userSessionRepository.SaveChangesAsync(cancellationToken);
            return new GoogleLoginDTO()
            {
                UserId = existingUser.Id,
                Email = existingUser.Email,
                DisplayName = existingUser.DisplayName ?? existingUser.Email?.Split('@')[0],
                AvatarUrl = existingUser.AvatarUrl,
                IsNewUser = false
            };
        }

        var user = new User()
        {
            Id = Guid.NewGuid(),
            GoogleId = request.GoogleId,
            Email = request.Email,
            AuthProvider = "Google",
            DisplayName = request.DisplayName ?? request.Email?.Split('@')[0],
            AvatarUrl = request.AvatarUrl,
            CreatedAt = DateTime.UtcNow
        };

        var stats = new UserStat()
        {
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow
        };

        user.Stats = stats;
        await userRepository.AddAsync(user, cancellationToken);
        
        var firstSession = new UserSession
        {
            UserId = user.Id,
            LoginAt = DateTime.UtcNow,
            IsActive = true,
            DeviceType = request.DeviceType,
            IpAddress = request.IpAddress,
            UserAgent = request.UserAgent
        };

        await userSessionRepository.AddAsync(firstSession, cancellationToken);
        await userSessionRepository.SaveChangesAsync(cancellationToken);

        return new GoogleLoginDTO()
        {
            UserId = user.Id,
            Email = user.Email,
            DisplayName = user.DisplayName,
            AvatarUrl = user.AvatarUrl,
            IsNewUser = true
        };
    }
}