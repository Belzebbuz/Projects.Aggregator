using Application.Contracts.Services.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using SharedLibrary.ApiMessages.Identity.Base;
using SharedLibrary.ApiMessages.Identity.M004;
using SharedLibrary.ApiMessages.Identity.M005;
using SharedLibrary.ApiMessages.Identity.M006;
using SharedLibrary.ApiMessages.Identity.M007;
using SharedLibrary.ApiMessages.Identity.M008;
using SharedLibrary.Authentication;
using SharedLibrary.Routes;
using SharedLibrary.Wrapper;
using IResult = SharedLibrary.Wrapper.IResult;

namespace Host.Controllers.Identity
{
	[Route(UsersEndpoints.Base)]
	[ApiController]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = SHRoles.Admin)]
	public class UsersController : ControllerBase
	{
		private readonly IUserService _userService;

		public UsersController(IUserService userService) => _userService = userService;

		[HttpGet]
		[OpenApiOperation("Get list of all users.", "")]
		public async Task<PaginatedResult<UserDto>> GetListAsync()
			=> await _userService.GetListAsync();

		[HttpGet("{id}")]
		[OpenApiOperation("Get a user's details.", "")]
		public async Task<IResult<M007Response>> GetByIdAsync(string id)
			=> await _userService.GetAsync(id);

		[HttpGet("{id}/roles")]
		[OpenApiOperation("Get a user's roles.", "")]
		public async Task<IResult<M004Response>> GetRolesAsync(string id)
			=> await _userService.GetRolesAsync(id);

		[HttpPost]
		[OpenApiOperation("Creates a new user.", "")]
		public async Task<IResult> CreateAsync(M005Request request)
		=> await _userService.CreateAsync(request, GetOriginFromRequest());

		[HttpPost("self-register")]
		[AllowAnonymous]
		[OpenApiOperation("Anonymous user creates a user.", "")]
		public async Task<IResult> SelfRegisterAsync(M005Request request)
			=> await _userService.CreateAsync(request, GetOriginFromRequest());

		[HttpGet("{id}/toogle-status")]
		[OpenApiOperation("Get list of all users.", "")]
		public async Task<IResult> ToogleUserStatus(M006Request request)
			=> await _userService.ToggleStatusAsync(request);

		[HttpPost("roles")]
		[OpenApiOperation("Update a user's assigned roles.", "")]
		public async Task<IResult> AssignRolesAsync(M008Request request)
			=> await _userService.AssignRolesAsync(request);

		private string GetOriginFromRequest() => $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
	}
}
