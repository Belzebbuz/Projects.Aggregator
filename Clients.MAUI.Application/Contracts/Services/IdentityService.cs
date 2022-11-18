using Clients.MAUI.Application.Contracts.Services.Common;
using SharedLibrary.ApiMessages.Identity.Base;
using SharedLibrary.ApiMessages.Identity.ID003;
using SharedLibrary.ApiMessages.Identity.ID005;
using SharedLibrary.ApiMessages.Identity.ID008;
using SharedLibrary.ApiMessages.Identity.ID009;
using SharedLibrary.Wrapper;

namespace Clients.MAUI.Application.Contracts.Services;

public interface IIdentityService : ITransientService
{
    public Task<IResult> SelfRegisterAsync(ID005Request request);
    public Task<PaginatedResult<UserDto>> SearchUsersAsync(string name = "", int page = 1, int itemsPerPage = 5);
    public Task<IResult> ToggleUserStatusAsync(string userId, bool isActive);
    public Task<IResult> AssignUserRolesAsync(ID008Request request);
    public Task<IResult<UserDto>> GetUserAsync(string userId);
    public Task<IResult> CreateUserAsync(ID005Request request);
    public Task<IResult<List<UserRoleDto>>> GetUserRoleAsync(string userId);
    public Task<IResult> ChangePasswordAsync(ID009Request request);
    Task<IResult> DeleteUserAsync(string id);
}
