namespace Harfistan.Application.DTOs.Auths;

public record AnonymousUserDTO(
    int UserId,
    bool IsNewUser, 
    string DeviceId = "",
    string DisplayName = ""
);