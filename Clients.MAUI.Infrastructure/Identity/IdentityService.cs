using Clients.MAUI.Application.Contracts.Services;
using Clients.MAUI.Infrastructure.Extensions;
using SharedLibrary.ApiMessages.Identity.Base;
using SharedLibrary.ApiMessages.Identity.ID003;
using SharedLibrary.ApiMessages.Identity.ID004;
using SharedLibrary.ApiMessages.Identity.ID005;
using SharedLibrary.ApiMessages.Identity.ID006;
using SharedLibrary.ApiMessages.Identity.ID008;
using SharedLibrary.ApiMessages.Identity.ID009;
using SharedLibrary.ApiMessages.Identity.ID010;
using SharedLibrary.Routes;
using SharedLibrary.Wrapper;
using System.Net.Http.Json;

namespace Clients.MAUI.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
	private readonly HttpClient _client;

	public IdentityService(HttpClient client) => _client = client;
	public async Task<IResult> AssignUserRolesAsync(ID008Request request)
	{
		var response = await _client.PostAsJsonAsync(UsersEndpoints.GetRolesRoute(), request);
		return await response.ToResult();
	}

	public async Task<IResult> ChangePasswordAsync(ID009Request request)
	{
		var response = await _client.PostAsJsonAsync(UsersEndpoints.GetChangePasswordRoute(), request);
		return await response.ToResult();
	}


	public async Task<IResult> CreateUserAsync(ID005Request request)
	{
		var response = await _client.PostAsJsonAsync(UsersEndpoints.Base, request);
		return await response.ToResult();
	}

	public async Task<IResult> DeleteUserAsync(string id)
	{
		var response = await _client.DeleteAsync(UsersEndpoints.GetUserRoute(id));
		return await response.ToResult();
	}

	public async Task<IResult<UserDto>> GetUserAsync(string userId)
	{
		var response = await _client.GetAsync(UsersEndpoints.GetUserRoute(userId));
		return await response.ToResult<UserDto>();
	}

	public async Task<IResult<List<UserRoleDto>>> GetUserRoleAsync(string userId)
	{
		var response = await _client.GetAsync(UsersEndpoints.GetUserRolesRoute(userId));
		var responseResult = await response.ToResult<ID004Response>();
		if (!responseResult.Succeeded)
			return Result<List<UserRoleDto>>.Fail(responseResult.Messages);

		return Result<List<UserRoleDto>>.Success(responseResult.Data.Roles);
	}

	public async Task<PaginatedResult<UserDto>> SearchUsersAsync(string name = "", int page = 1, int itemsPerPage = 5)
	{
		_client.AddOrUpdatePaginationHeaders(page, itemsPerPage);
		var response = await _client.PostAsJsonAsync(UsersEndpoints.GetUserByFilterRoute(), new ID010Request(name));
		return await response.ToPaginatedResult<UserDto>();
	}

	public async Task<IResult> SelfRegisterAsync(ID005Request request)
	{
		var response = await _client.PostAsJsonAsync(UsersEndpoints.GetSelfRegisterRoute(), request);
		return await response.ToResult();
	}

	public async Task<IResult> ToggleUserStatusAsync(string userId, bool isActive)
	{
		var response = await _client.PostAsJsonAsync(UsersEndpoints.GetToggleUserStatusRoute(), new ID006Request(userId, isActive));
		return await response.ToResult();
	}
}
