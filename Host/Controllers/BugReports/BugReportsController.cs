using Host.Controllers.Common;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.ApiMessages.BugReports.BG001;
using SharedLibrary.ApiMessages.BugReports.BG002;
using SharedLibrary.ApiMessages.BugReports.Dto;
using SharedLibrary.Authentication;
using SharedLibrary.Routes;
using SharedLibrary.Wrapper;
using IResult = SharedLibrary.Wrapper.IResult;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Host.Controllers.BugReports;

[Route(BugEndpoints.Base)]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BugReportsController : BaseApiController
{
    [HttpPost]
    public async Task<IResult> CreateBugReportAsync(BG001Request request)
		=> await Mediator.Send(request);

	[HttpGet]
	[Authorize(Roles = SHRoles.Admin)]
	public async Task<PaginatedResult<BugReportDto>> GetBugReportsAsync()
		=> await Mediator.Send(new BG002Request());
}
