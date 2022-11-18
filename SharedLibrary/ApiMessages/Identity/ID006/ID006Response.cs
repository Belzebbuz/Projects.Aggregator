namespace SharedLibrary.ApiMessages.Identity.ID006;

/// <summary>
/// Toggle user status request
/// </summary>
/// <param name="UserId"></param>
/// <param name="IsActive"></param>
public record ID006Request(string UserId, bool IsActive);


