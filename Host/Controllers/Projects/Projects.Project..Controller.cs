using Host.Controllers.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.ApiMessages.Projects.M009;
using SharedLibrary.ApiMessages.Projects.M010;
using SharedLibrary.ApiMessages.Projects.M011;
using SharedLibrary.ApiMessages.Projects.M012;
using SharedLibrary.ApiMessages.Projects.M018;
using SharedLibrary.ApiMessages.Projects.M020;
using SharedLibrary.ApiMessages.Projects.M022;
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
    public async Task<PaginatedResult<ProjectShortDto>> GetAllProjectsAsync()
        => await Mediator.Send(new M010Request());
	
    [HttpGet("names/{text}")]
	public async Task<IResult<ICollection<string>>> GetAllProjectsAsync(string text)
		=> await Mediator.Send(new M022Request(text));
	
    [HttpPost("filter")]
    public async Task<PaginatedResult<ProjectShortDto>> GetProjectsByFilter(M020Request request)
        => await Mediator.Send(request);

    [HttpPost]
    [Authorize(Roles = SHRoles.Dev)]
    public async Task<IResult> InitialCreateProjectAsync(M011Request request)
        => await Mediator.Send(request);

    [HttpPut]
    [Authorize(Roles = SHRoles.Dev)]
    public async Task<IResult> UpdateProjectAsync(M012Request request)
        => await Mediator.Send(request);

    [HttpGet("{id}")]
    public async Task<IResult<M009Response>> GetProjectByIdAsync(Guid id)
        => await Mediator.Send(new M009Request(id));

    [HttpDelete("{id}")]
    [Authorize(Roles = SHRoles.Dev)]
    public async Task<IResult> DeleteProjectAsync(Guid id)
        => await Mediator.Send(new M018Request(id));


}
