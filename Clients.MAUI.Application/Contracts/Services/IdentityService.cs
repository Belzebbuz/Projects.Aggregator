using Clients.MAUI.Application.Contracts.Services.Common;
using SharedLibrary.ApiMessages.Identity.Base;
using SharedLibrary.ApiMessages.Identity.M003;
using SharedLibrary.ApiMessages.Identity.M005;
using SharedLibrary.ApiMessages.Identity.M008;
using SharedLibrary.Wrapper;

namespace Clients.MAUI.Application.Contracts.Services;

public interface IIdentityService : ITransientService
{
    public Task<IResult> SelfRegisterAsync(M005Request request);
    public Task<IResult<M003Response>> GetUsersListAsync();
    public Task<IResult> ToogleUserStatusAsync(string userId, bool isActive);
    public Task<IResult> AssignUserRolesAsync(M008Request request);
    public Task<IResult<UserDto>> GetUserAsync(string userId);
    public Task<IResult> CreateUserAsync(M005Request request);
    public Task<IResult<UserRoleDto>> GetUserRoleAsync(string userId);
}
