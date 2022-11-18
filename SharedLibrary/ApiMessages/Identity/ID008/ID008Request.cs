using SharedLibrary.ApiMessages.Identity.Base;

namespace SharedLibrary.ApiMessages.Identity.ID008;

/// <summary>
/// Assign roles request
/// </summary>
public record ID008Request(string UserId, List<UserRoleDto> Roles);
