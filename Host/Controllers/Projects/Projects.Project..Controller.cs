using Host.Controllers.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.ApiMessages.Projects.P001;
using SharedLibrary.ApiMessages.Projects.P002;
using SharedLibrary.ApiMessages.Projects.P003;
using SharedLibrary.ApiMessages.Projects.P004;
using SharedLibrary.ApiMessages.Projects.P010;
using SharedLibrary.ApiMessages.Projects.P012;
using SharedLibrary.ApiMessages.Projects.P014;
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
        => await Mediator.Send(new P002Request());
	
    [HttpGet("names/{text}")]
	public async Task<IResult<ICollection<string>>> GetAllProjectsAsync(string text)
		=> await Mediator.Send(new P014Request(text));
	
    [HttpPost("filter")]
    public async Task<PaginatedResult<ProjectShortDto>> GetProjectsByFilter(P012Request request)
        => await Mediator.Send(request);

    [HttpPost]
    [Authorize(Roles = SHRoles.Dev)]
    public async Task<IResult> InitialCreateProjectAsync(P003Request request)
        => await Mediator.Send(request);

    [HttpPut]
    [Authorize(Roles = SHRoles.Dev)]
    public async Task<IResult> UpdateProjectAsync(P004Request request)
        => await Mediator.Send(request);

    [HttpGet("{id}")]
    public async Task<IResult<P001Response>> GetProjectByIdAsync(Guid id)
        => await Mediator.Send(new P001Request(id));

    [HttpDelete("{id}")]
    [Authorize(Roles = SHRoles.Dev)]
    public async Task<IResult> DeleteProjectAsync(Guid id)
        => await Mediator.Send(new P010Request(id));


}
