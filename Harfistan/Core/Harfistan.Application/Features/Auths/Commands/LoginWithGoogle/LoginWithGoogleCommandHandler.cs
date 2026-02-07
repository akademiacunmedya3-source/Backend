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
}

// public class LoginWithGoogleCommandHandler(IQueryRepository<User> queryRepository, ICommandRepository<User> commandRepository) : IRequestHandler<LoginWithGoogleCommand, GoogleLoginDTO>
// {
//     public async ValueTask<GoogleLoginDTO> Handle(LoginWithGoogleCommand request, CancellationToken cancellationToken)
//     {
//         var existingUser = await queryRepository.Table.Where(x => x.GoogleId == request.GoogleId).FirstOrDefaultAsync();
//         if (existingUser is not null)
//         {
//         }
//     }
// }