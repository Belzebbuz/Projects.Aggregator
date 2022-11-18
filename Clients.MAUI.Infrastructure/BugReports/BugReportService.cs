using Clients.MAUI.Application.Contracts.Services;
using Clients.MAUI.Infrastructure.Extensions;
using SharedLibrary.ApiMessages.BugReports.BG001;
using SharedLibrary.ApiMessages.BugReports.Dto;
using SharedLibrary.Routes;
using SharedLibrary.Wrapper;
using System.Net.Http.Json;

namespace Clients.MAUI.Infrastructure.BugReports;

public class BugReportService : IBugReportService
{
	private readonly HttpClient _client;

	public BugReportService(HttpClient client) => _client = client;
	public async Task<PaginatedResult<BugReportDto>> GetBugReportsAsync(int page = 1, int itemsPerPage = 5)
	{
		_client.AddOrUpdatePaginationHeaders(page, itemsPerPage);
		var response = await _client.GetAsync(BugEndpoints.Base);
		return await response.ToPaginatedResult<BugReportDto>();
	}

	public async Task<IResult> SendBugReportAsync(BG001Request request)
	{
		var reponse = await _client.PostAsJsonAsync(BugEndpoints.Base, request);
		return await reponse.ToResult();
	}
}
