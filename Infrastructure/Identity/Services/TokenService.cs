using Application.Contracts.Services.Identity;
using Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.ApiMessages.Identity.M001;
using SharedLibrary.ApiMessages.Identity.M002;
using SharedLibrary.Authentication;
using SharedLibrary.Exceptions;
using SharedLibrary.Wrapper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Identity.Services;

public class TokenService : ITokenService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IStringLocalizer<TokenService> _localizer;
    private readonly SecuritySettings _securitySettings;
    private readonly JwtSettings _jwtSettings;
    public TokenService(
        UserManager<AppUser> userManager,
        IOptions<JwtSettings> jwtSettings,
        IStringLocalizer<TokenService> localizer,
        IOptions<SecuritySettings> securitySettings)
    {
        _userManager = userManager;
        _localizer = localizer;
        _jwtSettings = jwtSettings.Value;
        _securitySettings = securitySettings.Value;
    }
    public async Task<IResult<M001Response>> GetTokenAsync(M001Request request, string ipAddress, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email.Trim().Normalize());
        if (user is null)
        {
            return await Result<M001Response>.FailAsync(_localizer["auth.failed"]);
        }

        if (!user.IsActive)
        {
            return await Result<M001Response>.FailAsync(_localizer["identity.usernotactive"]);
        }

        if (_securitySettings.RequireConfirmedAccount && !user.EmailConfirmed)
        {
            return await Result<M001Response>.FailAsync(_localizer["identity.emailnotconfirmed"]);
        }

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return await Result<M001Response>.FailAsync(_localizer["identity.invalidcredentials"]);
        }

        return await Result<M001Response>.SuccessAsync(await GenerateTokensAndUpdateUser(user, ipAddress));
    }

    public async Task<IResult<M001Response>> RefreshTokenAsync(M002Request request, string ipAddress)
    {
        var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
        string? userEmail = userPrincipal.GetEmail();
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user is null)
        {
            return await Result<M001Response>.FailAsync(_localizer["auth.failed"]);
        }

        if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return await Result<M001Response>.FailAsync(_localizer["identity.invalidrefreshtoken"]);
        }

        return await Result<M001Response>.SuccessAsync(await GenerateTokensAndUpdateUser(user, ipAddress));
    }

    private async Task<M001Response> GenerateTokensAndUpdateUser(AppUser user, string ipAddress)
    {
        var roles = await _userManager.GetRolesAsync(user);
        string token = GenerateJwt(user, ipAddress, roles);

        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.ExpirationInDays);

        await _userManager.UpdateAsync(user);

        return new M001Response(token, user.RefreshToken, user.RefreshTokenExpiryTime);
    }

    private string GenerateJwt(AppUser user, string ipAddress, IList<string> roles) =>
       GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user, ipAddress, roles));

    private IEnumerable<Claim> GetClaims(AppUser user, string ipAddress, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.FirstName ?? string.Empty),
            new(ClaimTypes.Surname, user.LastName ?? string.Empty),
            new(SHClaims.IpAddress, ipAddress),
            new(SHClaims.ImageUrl, user.ImageUrl ?? string.Empty),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
        };

        foreach (var role in roles)
        {
            claims.Add(new(ClaimTypes.Role, role));
        }
        return claims;
    }


    private string GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(_jwtSettings.ExpirationInDays),
            signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        if (string.IsNullOrEmpty(_jwtSettings.Key))
        {
            throw new InvalidOperationException("No Key defined in JwtSettings config.");
        }

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new UnauthorizedException(_localizer["identity.invalidtoken"]);
        }

        return principal;
    }

    private SigningCredentials GetSigningCredentials()
    {
        if (string.IsNullOrEmpty(_jwtSettings.Key))
        {
            throw new InvalidOperationException("No Key defined in JwtSettings config.");
        }

        byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }
}
