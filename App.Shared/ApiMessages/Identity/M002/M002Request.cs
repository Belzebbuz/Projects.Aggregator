namespace App.Shared.ApiMessages.Identity.M002;

/// <summary>
/// Refresh token response
/// </summary>
/// <param name="Token"></param>
/// <param name="RefreshToken"></param>
public record M002Request(string Token, string RefreshToken);

