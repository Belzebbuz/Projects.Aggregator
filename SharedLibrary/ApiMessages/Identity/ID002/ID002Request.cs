namespace SharedLibrary.ApiMessages.Identity.ID002;

/// <summary>
/// Refresh token response
/// </summary>
/// <param name="Token"></param>
/// <param name="RefreshToken"></param>
public record ID002Request(string Token, string RefreshToken);

