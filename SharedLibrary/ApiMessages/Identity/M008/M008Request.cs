using SharedLibrary.ApiMessages.Identity.Base;

namespace SharedLibrary.ApiMessages.Identity.M008;

/// <summary>
/// Assign roles request
/// </summary>
public record M008Request(string UserId, List<UserRoleDto> Roles);
