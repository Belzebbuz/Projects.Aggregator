using Clients.MAUI.Application.Contracts.Services.Common;
using SharedLibrary.Wrapper;

namespace Clients.MAUI.Application.Contracts.Services;

public interface IAppInfoService : ITransientService
{
	public Task<IResult<string>> GetServerVersionAsync();
	public Task SetServerUrlAsync(string serverURL);
	public Task<string> GetServerUrlAsync();
}
