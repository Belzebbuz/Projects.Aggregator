using SharedLibrary.ApiMessages.Identity.Base;

namespace SharedLibrary.ApiMessages.Identity.ID004;

/// <summary>
/// List of roles dto
/// </summary>
/// <param name="Roles"></param>
public record ID004Response(List<UserRoleDto> Roles);

