using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.ApiMessages.Projects.M013;
using SharedLibrary.ApiMessages.Projects.M014;
using SharedLibrary.ApiMessages.Projects.M017;
using SharedLibrary.ApiMessages.Projects.M019;
using SharedLibrary.Authentication;
using SharedLibrary.Routes;
using IResult = SharedLibrary.Wrapper.IResult;

namespace Host.Controllers.Projects;

/// <summary>
/// API формирования запросов <see cref="ProjectsEndpoints"/>
/// </summary>
public partial class ProjectsController
{
	[HttpPost("{projectId}/releases")]
	[RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
	[RequestSizeLimit(long.MaxValue)]
	[Authorize(Roles = SHRoles.Dev)]
	public async Task<IResult> AddReleaseAsync(Guid projectId, IFormFile file)
	   => await Mediator.Send(new M013Request(projectId, file.FileName, file.OpenReadStream()));

	[HttpDelete("{projectId}/releases/{releaseId}")]
	[Authorize(Roles = SHRoles.Dev)]
	public async Task<IResult> DeleteReleaseAsync(Guid projectId, Guid releaseId)
		=> await Mediator.Send(new M014Request(projectId, releaseId));

	[HttpGet("{projectId}/releases/{releaseId}")]
	public async Task<IActionResult> DownloadFileAsync(Guid projectId, Guid releaseId)
	{
		var result = await Mediator.Send(new M017Request(projectId, releaseId));
		if (!result.Succeeded)
			return BadRequest(result);

		return File(result.Data.FileStream, result.Data.ContentType);
	}

	[HttpPost("releaseNotes")]
	[Authorize(Roles = SHRoles.Dev)]
	public async Task<IResult> AddOrUpdateReleaseNoteAsync(M019Request request)
		=> await Mediator.Send(request);
}
