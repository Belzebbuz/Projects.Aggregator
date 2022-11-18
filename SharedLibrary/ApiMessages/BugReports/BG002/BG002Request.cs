using MediatR;
using SharedLibrary.ApiMessages.BugReports.Dto;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.BugReports.BG002;

public class BG002Request : IRequest<PaginatedResult<BugReportDto>>
{

}
