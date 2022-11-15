using Application.Contracts.DI;
using SharedLibrary.ApiMessages.Identity.Base;
using SharedLibrary.ApiMessages.Identity.M004;
using SharedLibrary.ApiMessages.Identity.M005;
using SharedLibrary.ApiMessages.Identity.M006;
using SharedLibrary.ApiMessages.Identity.M007;
using SharedLibrary.ApiMessages.Identity.M008;
using SharedLibrary.Wrapper;

namespace Application.Contracts.Services.Identity;

public interface IUserService : IScopedService
{
    Task<IResult> AssignRolesAsync(M008Request request);
    Task<IResult> CreateAsync(M005Request request, string origin);
    Task<IResult<M007Response>> GetAsync(string id);
    Task<PaginatedResult<UserDto>> GetListAsync();
    Task<IResult<M004Response>> GetRolesAsync(string id);
    Task<IResult> ToggleStatusAsync(M006Request request);
}
