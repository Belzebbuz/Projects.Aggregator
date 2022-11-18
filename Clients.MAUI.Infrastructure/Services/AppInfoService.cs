using Clients.MAUI.Application.Contracts.Services;
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

	public async Task<IResult<string>> GetServerVersionAsync()
	{
		var response = await _client.GetAsync(CheckEndpoints.Base);
		return await response.ToResult<string>();
	}
}
