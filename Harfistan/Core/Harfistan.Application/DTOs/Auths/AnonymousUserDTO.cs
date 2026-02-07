namespace Harfistan.Application.DTOs.Auths;

public record AnonymousUserDTO(
    Guid UserId,
    bool IsNewUser, 
    string DeviceId = "",
    string DisplayName = ""
);