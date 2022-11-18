using Application.Contracts.DI;
using SharedLibrary.ApiMessages.Identity.Base;
using SharedLibrary.ApiMessages.Identity.ID004;
using SharedLibrary.ApiMessages.Identity.ID005;
using SharedLibrary.ApiMessages.Identity.ID006;
using SharedLibrary.ApiMessages.Identity.ID007;
using SharedLibrary.ApiMessages.Identity.ID009;
using SharedLibrary.ApiMessages.Identity.ID008;
using SharedLibrary.Wrapper;
using SharedLibrary.ApiMessages.Identity.ID010;

namespace Application.Contracts.Services.Identity;

public interface IUserService : IScopedService
{
    Task<IResult> AssignRolesAsync(ID008Request request);
    Task<IResult> CreateAsync(ID005Request request, string origin);
    Task<IResult<ID007Response>> GetAsync(string id);
    Task<PaginatedResult<UserDto>> GetListAsync();
    Task<IResult<ID004Response>> GetRolesAsync(string id);
    Task<IResult> ToggleStatusAsync(ID006Request request);
    Task<IResult> ChangePasswordAsync(ID009Request request, string userId);
    Task<PaginatedResult<UserDto>> GetListByFilterAsync(ID010Request request);
    Task<IResult> DeleteAsync(string id);
}
