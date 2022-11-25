using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.ApiMessages.Projects.P013;
using SharedLibrary.Authentication;
using SharedLibrary.Wrapper;
using SharedLibrary.Routes;
using IResult = SharedLibrary.Wrapper.IResult;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.ApiMessages.Projects.P015;
using SharedLibrary.ApiMessages.Projects.P016;
using SharedLibrary.ApiMessages.Projects.P008;
using NSwag.Annotations;
using SharedLibrary.ApiMessages.Projects.P007;

namespace Host.Controllers.Projects;
/// <summary>
/// API формирования запросов  <see cref="ProjectsEndpoints"/>
/// </summary>
public partial class ProjectsController
{
	[HttpGet("tags")]
	[OpenApiOperation("Получить список всех тегов", "")]
	public async Task<IResult<P013Response>> GetAllTagsAsync()
		=> await Mediator.Send(new P013Request());

	[HttpGet("tags/{text}")]
	[OpenApiOperation("Получить список тегов по имени", "")]
	public async Task<IResult<List<TagDto>>> GetTagsByFilterAsync(string? text)
		=> await Mediator.Send(new P015Request(text));

	[HttpPost("tags")]
	[Authorize(Roles = SHRoles.Dev)]
	[OpenApiOperation("Создать новый тег", "")]
	public async Task<IResult<Guid>> CreateTagAsync(P016Request request)
		=> await Mediator.Send(request);

	[HttpPut("tags")]
	[Authorize(Roles = SHRoles.Dev)]
	[OpenApiOperation("Добавить тег к проекту", "")]
	public async Task<IResult> AddTagToProjectAsync(P007Request request)
		=> await Mediator.Send(request);

	[HttpDelete("{projectId}/tags/{tagId}")]
	[Authorize(Roles = SHRoles.Dev)]
	[OpenApiOperation("Удалить тег из проекта", "")]
	public async Task<IResult> DeleteTagAsync(Guid projectId, Guid tagId)
		=> await Mediator.Send(new P008Request(projectId, tagId));
}
