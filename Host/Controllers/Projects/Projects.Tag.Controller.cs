using App.Shared.ApiMessages.Projects.M015;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.ApiMessages.Projects.M021;
using SharedLibrary.Authentication;
using SharedLibrary.Wrapper;
using SharedLibrary.Routes;
using IResult = SharedLibrary.Wrapper.IResult;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.ApiMessages.Projects.M022;
using SharedLibrary.ApiMessages.Projects.M023;
using SharedLibrary.ApiMessages.Projects.M024;
using SharedLibrary.ApiMessages.Projects.M025;

namespace Host.Controllers.Projects;
/// <summary>
/// API формирования запросов  <see cref="ProjectsEndpoints"/>
/// </summary>
public partial class ProjectsController
{
	[HttpGet("tags")]
	public async Task<IResult<M021Response>> GetAllTagsAsync()
		=> await Mediator.Send(new M021Request());

	[HttpGet("tags/{text}")]
	public async Task<IResult<List<TagDto>>> GetTagsByFilterAsync(string? text)
		=> await Mediator.Send(new M023Request(text));

	[HttpPost("tags")]
	[Authorize(Roles = SHRoles.Dev)]
	public async Task<IResult<Guid>> CreateTagAsync(M024Request request)
		=> await Mediator.Send(request);

	[HttpPost("multiple_tags")]
	[Authorize(Roles = SHRoles.Dev)]
	public async Task<IResult<List<Guid>>> CreateMultipleTagsAsync(M025Request request)
		=> await Mediator.Send(request);

	[HttpPut("tags")]
	[Authorize(Roles = SHRoles.Dev)]
	public async Task<IResult> AddTagToProjectAsync(M015Request request)
		=> await Mediator.Send(request);

	[HttpDelete("{projectId}/tags/{tagId}")]
	[Authorize(Roles = SHRoles.Dev)]
	public async Task<IResult> DeleteTagAsync(Guid projectId, Guid tagId)
		=> await Mediator.Send(new M016Request(projectId, tagId));
}
