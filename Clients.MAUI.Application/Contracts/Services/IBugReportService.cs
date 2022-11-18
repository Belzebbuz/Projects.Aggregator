using Clients.MAUI.Application.Contracts.Services.Common;
using SharedLibrary.ApiMessages.BugReports.BG001;
using SharedLibrary.ApiMessages.BugReports.BG002;
using SharedLibrary.ApiMessages.BugReports.Dto;
using SharedLibrary.Wrapper;

namespace Clients.MAUI.Application.Contracts.Services;

public interface IBugReportService : ITransientService
{
	public Task<IResult> SendBugReportAsync(BG001Request request);
	public Task<PaginatedResult<BugReportDto>> GetBugReportsAsync(int page = 1, int itemsPerPage = 5);
}
