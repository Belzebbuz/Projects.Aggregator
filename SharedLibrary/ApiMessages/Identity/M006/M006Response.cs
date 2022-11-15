namespace SharedLibrary.ApiMessages.Identity.M006;

/// <summary>
/// Toogle user status request
/// </summary>
/// <param name="UserId"></param>
/// <param name="IsActive"></param>
public record M006Request(string UserId, bool IsActive);


