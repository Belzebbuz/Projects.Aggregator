namespace App.Shared.ApiMessages.Identity.M001;

/// <summary>
/// Token response message
/// </summary>
/// <param name="Token"></param>
/// <param name="RefreshToken"></param>
/// <param name="RefreshTokenExpiryTime"></param>
public record M001Response(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);

