using Clients.MAUI.Application.Contracts.Services;
using SharedLibrary.ApiMessages.Identity.Base;
using SharedLibrary.ApiMessages.Identity.M003;
using SharedLibrary.ApiMessages.Identity.M005;
using SharedLibrary.ApiMessages.Identity.M006;
using SharedLibrary.ApiMessages.Identity.M008;
using SharedLibrary.Routes;
using SharedLibrary.Wrapper;
using System.Net.Http.Json;

namespace Clients.MAUI.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly HttpClient _client;

    public IdentityService(HttpClient client) => _client = client;
    public async Task<IResult> AssignUserRolesAsync(M008Request request)
    {
        var response = await _client.PostAsJsonAsync(UsersEndpoints.AssignRoles(), request);
        return await response.ToResult();
    }

    public async Task<IResult> CreateUserAsync(M005Request request)
    {
        var response = await _client.PostAsJsonAsync(UsersEndpoints.Base, request);
        return await response.ToResult();
    }

    public async Task<IResult<UserDto>> GetUserAsync(string userId)
    {
        var response = await _client.GetAsync(UsersEndpoints.GetUser(userId));
        return await response.ToResult<UserDto>();
    }

    public async Task<IResult<UserRoleDto>> GetUserRoleAsync(string userId)
    {
        var response = await _client.GetAsync(UsersEndpoints.GetUserRoles(userId));
        return await response.ToResult<UserRoleDto>();
    }

    public async Task<IResult<M003Response>> GetUsersListAsync()
    {
        var response = await _client.GetAsync(UsersEndpoints.Base);
        return await response.ToResult<M003Response>();
    }

    public async Task<IResult> SelfRegisterAsync(M005Request request)
    {
        var response = await _client.PostAsJsonAsync(UsersEndpoints.SelfRegister(), request);
        return await response.ToResult();
    }

    public async Task<IResult> ToogleUserStatusAsync(string userId, bool isActive)
    {
        var response = await _client.PostAsJsonAsync(UsersEndpoints.ToogleUserStatus(), new M006Request(userId, isActive));
        return await response.ToResult();
    }
}
