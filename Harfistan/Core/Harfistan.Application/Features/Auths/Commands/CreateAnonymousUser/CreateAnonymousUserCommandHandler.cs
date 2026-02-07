using Harfistan.Application.Abstractions.Repositories;
using Harfistan.Application.Commons;
using Harfistan.Application.DTOs.Auths;
using Harfistan.Domain.Entities;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Harfistan.Application.Features.Auths.Commands.CreateAnonymousUser;

public record CreateAnonymousUserCommand(string? DeviceType, string? DeviceModel, string DeviceId="") : IRequest<AnonymousUserDTO>;

public class CreateAnonymousUserCommandHandler(IUserRepository _userRepository, IUserSessionRepository _userSessionRepository) : IRequestHandler<CreateAnonymousUserCommand, AnonymousUserDTO>
{
    public async ValueTask<AnonymousUserDTO> Handle(CreateAnonymousUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.Table.Where(u => u.DeviceId == request.DeviceId).FirstOrDefaultAsync(cancellationToken);

        if (existingUser != null)
        {
            var existingSession = new UserSession()
            {
                UserId = existingUser.Id,
                LoginAt = DateTime.UtcNow,
                IsActive = true,
                DeviceType = request.DeviceType,
                DeviceModel = request.DeviceModel
            };
            await _userSessionRepository.AddAsync(existingSession, cancellationToken);
            await _userSessionRepository.SaveChangesAsync(cancellationToken);
            
            return new AnonymousUserDTO(existingUser.Id, false, existingUser.DeviceId, existingUser.DisplayName ?? "Player");
        }

        User user = new()
        {
            Id = Guid.NewGuid(),
            DeviceId = request.DeviceId,
            AuthProvider = "Anonymous",
            DisplayName = $"Player_{new Random().Next(1000, 9999)}",
            CreatedAt = DateTime.UtcNow
        };

        var stats = new UserStat()
        {
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow
        };
        user.Stats = stats;

        await _userRepository.AddAsync(user, cancellationToken);

        var firstSession = new UserSession()
        {
            UserId = user.Id,
            LoginAt = DateTime.UtcNow,
            IsActive = true,
            DeviceType = request.DeviceType,
            DeviceModel = request.DeviceModel
        };
        
        await _userSessionRepository.AddAsync(firstSession, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);
        
        return new AnonymousUserDTO(user.Id, true, user.DeviceId, user.DisplayName);
    }
}