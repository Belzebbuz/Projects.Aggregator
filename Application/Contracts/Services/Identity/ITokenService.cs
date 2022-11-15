using Application.Contracts.DI;
using SharedLibrary.ApiMessages.Identity.M001;
using SharedLibrary.ApiMessages.Identity.M002;
using SharedLibrary.Wrapper;

namespace Application.Contracts.Services.Identity;

public interface ITokenService : IScopedService
{
    Task<IResult<M001Response>> GetTokenAsync(M001Request request, string ipAddress, CancellationToken cancellationToken);
    Task<IResult<M001Response>> RefreshTokenAsync(M002Request request, string ipAddress);
}
