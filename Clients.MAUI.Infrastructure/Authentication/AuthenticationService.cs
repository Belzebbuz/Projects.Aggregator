using Clients.MAUI.Application.Contracts.Services;
using Clients.MAUI.Infrastructure.Constants;
using SharedLibrary.ApiMessages.Identity.M001;
using SharedLibrary.Routes;
using SharedLibrary.Wrapper;
using System.Net.Http.Json;

namespace Clients.MAUI.Infrastructure.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _client;
    private readonly LocalAuthenticationStateProvider _authenticationStateProvider;

    public AuthenticationService(HttpClient client, LocalAuthenticationStateProvider authenticationStateProvider)
    {
        _client = client;
        _authenticationStateProvider = authenticationStateProvider;
    }
    public async Task<IResult> LoginAsync(M001Request request)
    {
        try
        {
            var tokenResponse = await _client.PostAsJsonAsync(TokenEndpoints.Base, request);
            var tokenResult = await tokenResponse.ToResult<M001Response>();
            if (!tokenResult.Succeeded)
                return tokenResult;

            var token = tokenResult.Data.Token;
            var refreshToken = tokenResult.Data.RefreshToken;
            await SecureStorage.SetAsync(StorageConstants.AuthToken, token);
            await SecureStorage.SetAsync(StorageConstants.RefreshToken, refreshToken);
            await _authenticationStateProvider.GetAuthenticationStateAsync();
            return await Result.SuccessAsync();
        }
        catch (Exception ex)
        {
            return await Result.FailAsync(ex.Message);
        }

    }

    public async Task<IResult> LogoutAsync()
    {
        _authenticationStateProvider.MarkUserAsLoggedOut();
        return await Result.SuccessAsync();
    }
}
