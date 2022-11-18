using Application.Contracts.DI;
using SharedLibrary.ApiMessages.Identity.ID001;
using SharedLibrary.ApiMessages.Identity.ID002;
using SharedLibrary.Wrapper;

namespace Application.Contracts.Services.Identity;

public interface ITokenService : IScopedService
{
    Task<IResult<ID001Response>> GetTokenAsync(ID001Request request, string ipAddress, CancellationToken cancellationToken);
    Task<IResult<ID001Response>> RefreshTokenAsync(ID002Request request, string ipAddress);
}
