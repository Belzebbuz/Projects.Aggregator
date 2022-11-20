using Clients.MAUI.Application.Contracts.Services;
using Clients.MAUI.Infrastructure.Constants;
using SharedLibrary.Routes;
using SharedLibrary.Wrapper;

namespace Clients.MAUI.Infrastructure.Services;

public class AppInfoService : IAppInfoService
{
	private readonly HttpClient _client;

	public AppInfoService(HttpClient client)
	{
		_client = client;
	}

	public async Task<string> GetServerUrlAsync()
	{
		return await SecureStorage.GetAsync(StorageConstants.ServerURL);
	}

	public async Task<IResult<string>> GetServerVersionAsync()
	{
		var response = await _client.GetAsync(CheckEndpoints.Base);
		return await response.ToResult<string>();
	}

	public async Task SetServerUrlAsync(string serverURL)
	{
		await SecureStorage.SetAsync(StorageConstants.ServerURL, serverURL);
	}
}
