using SharedLibrary.ApiMessages.Identity.Base;

namespace SharedLibrary.ApiMessages.Identity.M004;

/// <summary>
/// List of roles dto
/// </summary>
/// <param name="Roles"></param>
public record M004Response(List<UserRoleDto> Roles);

