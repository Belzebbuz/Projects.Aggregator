using Application.Contracts.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using SharedLibrary.ApiMessages.Identity.M001;
using SharedLibrary.ApiMessages.Identity.M002;
using SharedLibrary.Routes;
using SharedLibrary.Wrapper;

namespace Host.Controllers.Identity;

[Route(TokenEndpoints.Base)]
[ApiController]
public class TokenController : ControllerBase
{
	private readonly ITokenService _tokenService;

	public TokenController(ITokenService tokenService) => _tokenService = tokenService;

	[HttpPost]
	[AllowAnonymous]
	[OpenApiOperation("Request an access token using credentials.", "")]
	public async Task<IResult<M001Response>> GetTokenAsync(M001Request request, CancellationToken cancellationToken)
		=> await _tokenService.GetTokenAsync(request, GetIpAddress(), cancellationToken);

	[HttpPost("refresh")]
	[AllowAnonymous]
	[OpenApiOperation("Request an access token using a refresh token.", "")]
	public async Task<IResult<M001Response>> RefreshAsync(M002Request request)
		=> await _tokenService.RefreshTokenAsync(request, GetIpAddress());

	private string GetIpAddress() =>
	   Request.Headers.ContainsKey("X-Forwarded-For")
		   ? Request.Headers["X-Forwarded-For"]
		   : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "N/A";
}
