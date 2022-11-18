namespace SharedLibrary.ApiMessages.Identity.ID001;

/// <summary>
/// Token response message
/// </summary>
/// <param name="Token"></param>
/// <param name="RefreshToken"></param>
/// <param name="RefreshTokenExpiryTime"></param>
public record ID001Response(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);

