using Host.Controllers.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.ApiMessages.Projects.P001;
using SharedLibrary.ApiMessages.Projects.P002;
using SharedLibrary.ApiMessages.Projects.P003;
using SharedLibrary.ApiMessages.Projects.P004;
using SharedLibrary.ApiMessages.Projects.P010;
using SharedLibrary.ApiMessages.Projects.P012;
using SharedLibrary.Authentication;
using SharedLibrary.Routes;
using SharedLibrary.Wrapper;
using IResult = SharedLibrary.Wrapper.IResult;

namespace Host.Controllers.Projects;
/// <summary>
/// API формирования запросов <see cref="ProjectsEndpoints"/>
/// </summary>
[Route(ProjectsEndpoints.Base)]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public partial class ProjectsController : BaseApiController
{
    [HttpGet]
    [OpenApiOperation("Получить список всех проектов." , OpenApiDescriptonConstants.PerPageDescription)]
    public async Task<PaginatedResult<ProjectShortDto>> GetAllProjectsAsync()
        => await Mediator.Send(new P002Request());
		
    [HttpPost("filter")]
	[OpenApiOperation("Получить список проектов по фильтру." , OpenApiDescriptonConstants.PerPageDescription)]
	public async Task<PaginatedResult<ProjectShortDto>> GetProjectsByFilter(P012Request request)
        => await Mediator.Send(request);

    [HttpPost]
	[OpenApiOperation("Создание проекта", "")]
	[Authorize(Roles = SHRoles.Dev)]
	public async Task<IResult> InitialCreateProjectAsync(P003Request request)
        => await Mediator.Send(request);

    [HttpPut]
	[OpenApiOperation("Обновить детали проекта", "")]
	[Authorize(Roles = SHRoles.Dev)]
	public async Task<IResult> UpdateProjectAsync(P004Request request)
        => await Mediator.Send(request);

    [HttpGet("{id}")]
	[OpenApiOperation("Получить подробное описание проекта", "")]
	public async Task<IResult<P001Response>> GetProjectByIdAsync(Guid id)
        => await Mediator.Send(new P001Request(id));

    [HttpDelete("{id}")]
	[OpenApiOperation("Удаление проекта со всеми релизами", "")]
	[Authorize(Roles = SHRoles.Dev)]
	public async Task<IResult> DeleteProjectAsync(Guid id)
        => await Mediator.Send(new P010Request(id));


}
