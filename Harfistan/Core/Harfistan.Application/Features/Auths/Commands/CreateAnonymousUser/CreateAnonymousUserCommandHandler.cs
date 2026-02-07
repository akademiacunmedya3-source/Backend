using Harfistan.Application.Abstractions.Repositories;
using Harfistan.Application.Commons;
using Harfistan.Application.DTOs.Auths;
using Harfistan.Domain.Entities;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Harfistan.Application.Features.Auths.Commands.CreateAnonymousUser;

public record CreateAnonymousUserCommand(string DeviceId="") : IRequest<AnonymousUserDTO>;

public class CreateAnonymousUserCommandHandler(ICommandRepository<User, Guid> _commandRepository, IQueryRepository<User, Guid> _queryRepository) : IRequestHandler<CreateAnonymousUserCommand, AnonymousUserDTO>
{
    public async ValueTask<AnonymousUserDTO> Handle(CreateAnonymousUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _queryRepository.Table.Where(u => u.DeviceId == request.DeviceId).FirstOrDefaultAsync(cancellationToken);

        if (existingUser != null)
        {
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

        await _commandRepository.AddAsync(user, cancellationToken);
        await _commandRepository.SaveAsync(cancellationToken);
        
        return new AnonymousUserDTO(user.Id, true, user.DeviceId, user.DisplayName);
    }
}