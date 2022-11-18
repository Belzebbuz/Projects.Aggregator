using Clients.MAUI.Application.Contracts.Services;
using Clients.MAUI.Infrastructure.Constants;
using Microsoft.AspNetCore.Components.Authorization;
using SharedLibrary.ApiMessages.Identity.ID001;
using SharedLibrary.Routes;
using SharedLibrary.Wrapper;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;

namespace Clients.MAUI.Infrastructure.Authentication;

public class AuthenticationService : IAuthenticationService
{
	private readonly HttpClient _client;
	private readonly AuthenticationStateProvider _authenticationStateProvider;

	public AuthenticationService(HttpClient client, AuthenticationStateProvider authenticationStateProvider)
	{
		_client = client;
		_authenticationStateProvider = authenticationStateProvider;
	}
	public async Task<IResult> LoginAsync(ID001Request request)
	{
		var tokenResponse = await _client.PostAsJsonAsync(TokenEndpoints.Base, request);
		var tokenResult = await tokenResponse.ToResult<ID001Response>();
		if (!tokenResult.Succeeded)
			return tokenResult;

		var token = tokenResult.Data.Token;
		var refreshToken = tokenResult.Data.RefreshToken;
		await SecureStorage.SetAsync(StorageConstants.AuthToken, token);
		await SecureStorage.SetAsync(StorageConstants.RefreshToken, refreshToken);
		((LocalAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(request.Email);
		_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		return await Result.SuccessAsync();
	}

	public async Task<IResult> LogoutAsync()
	{
		SecureStorage.Remove(StorageConstants.AuthToken);
		SecureStorage.Remove(StorageConstants.RefreshToken);
		((LocalAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
		_client.DefaultRequestHeaders.Authorization = null;
		return await Result.SuccessAsync();
	}
}
