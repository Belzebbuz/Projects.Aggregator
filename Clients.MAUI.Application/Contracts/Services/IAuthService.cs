using Clients.MAUI.Application.Contracts.Services.Common;
using SharedLibrary.ApiMessages.Identity.ID001;
using SharedLibrary.Wrapper;

namespace Clients.MAUI.Application.Contracts.Services;

public interface IAuthenticationService : ITransientService
{
    public Task<IResult> LoginAsync(ID001Request request);
    public Task<IResult> LogoutAsync();
}
