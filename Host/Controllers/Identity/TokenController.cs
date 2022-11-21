using Application.Contracts.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using SharedLibrary.ApiMessages.Identity.ID001;
using SharedLibrary.ApiMessages.Identity.ID002;
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
	[OpenApiOperation("Получить токен по электронной почте и паролю", "")]
	public async Task<IResult<ID001Response>> GetTokenAsync(ID001Request request, CancellationToken cancellationToken)
		=> await _tokenService.GetTokenAsync(request, GetIpAddress(), cancellationToken);

	[HttpPost("refresh")]
	[AllowAnonymous]
	public async Task<IResult<ID001Response>> RefreshAsync(ID002Request request)
		=> await _tokenService.RefreshTokenAsync(request, GetIpAddress());

	private string GetIpAddress() =>
	   Request.Headers.ContainsKey("X-Forwarded-For")
		   ? Request.Headers["X-Forwarded-For"]
		   : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "N/A";
}
