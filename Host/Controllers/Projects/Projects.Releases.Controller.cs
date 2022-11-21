using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using SharedLibrary.ApiMessages.Projects.P005;
using SharedLibrary.ApiMessages.Projects.P006;
using SharedLibrary.ApiMessages.Projects.P009;
using SharedLibrary.ApiMessages.Projects.P011;
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
	[OpenApiOperation("Загрузить файл с новым релизом", "")]
	public async Task<IResult> AddReleaseAsync(Guid projectId, IFormFile file)
	   => await Mediator.Send(new P005Request(projectId, file.FileName, file.OpenReadStream()));

	[HttpDelete("{projectId}/releases/{releaseId}")]
	[Authorize(Roles = SHRoles.Dev)]
	[OpenApiOperation("Удалить релиз и файла связанного с ним", "")]
	public async Task<IResult> DeleteReleaseAsync(Guid projectId, Guid releaseId)
		=> await Mediator.Send(new P006Request(projectId, releaseId));

	[HttpGet("{projectId}/releases/{releaseId}")]
	[OpenApiOperation("Скачать файл релиза", "")]
	public async Task<IActionResult> DownloadFileAsync(Guid projectId, Guid releaseId)
	{
		var result = await Mediator.Send(new P009Request(projectId, releaseId));
		if (!result.Succeeded)
			return BadRequest(result);

		return File(result.Data.FileStream, result.Data.ContentType);
	}

	[HttpPost("releaseNotes")]
	[Authorize(Roles = SHRoles.Dev)]
	[OpenApiOperation("Добавить/изменить описание релиза", "")]
	public async Task<IResult> AddOrUpdateReleaseNoteAsync(P011Request request)
		=> await Mediator.Send(request);
}
