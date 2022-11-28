using Host.Controllers.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.ApiMessages.Projects.P017;
using SharedLibrary.ApiMessages.Projects.P018;
using SharedLibrary.ApiMessages.Projects.P019;
using SharedLibrary.ApiMessages.Projects.P020;
using SharedLibrary.Authentication;
using SharedLibrary.Wrapper;
using IResult = SharedLibrary.Wrapper.IResult;

namespace Host.Controllers.Projects;

public partial class ProjectsController 
{
	[HttpGet("{projectId}/patchNotes/")]
	[Authorize(Roles = SHRoles.Admin)]
	[OpenApiOperation("Получить список патч-ноутов. ", OpenApiDescriptonConstants.PerPageDescription)]
	public async Task<PaginatedResult<PatchNoteDto>> GetPatchNoteAsync(Guid projectId)
	    => await Mediator.Send(new P020Request() { ProjectId = projectId});
	

	[HttpPost("{projectId}/patchNotes")]
    [Authorize(Roles = SHRoles.Admin)]
	[OpenApiOperation("Создать патч-ноут", "")]
	public async Task<IResult> CreatePatchNoteAsync(Guid projectId, P017Request request)
    {
        if (projectId != request.ProjectId)
            return Result.Fail($"Route id: {projectId}, but body request id was {request.ProjectId}");

        return await Mediator.Send(request);
    }

	[HttpPut("{projectId}/patchNotes")]
	[Authorize(Roles = SHRoles.Admin)]
	[OpenApiOperation("Изменить патч-ноут", "")]
	public async Task<IResult> UpdatePatchNoteAsync(Guid projectId, P018Request request)
	{
		if (projectId != request.ProjectId)
			return Result.Fail($"Route id: {projectId}, but body request id was {request.ProjectId}");

		return await Mediator.Send(request);
	}

	[HttpDelete("{projectId}/patchNotes/{patchNoteId}")]
	[Authorize(Roles = SHRoles.Admin)]
	[OpenApiOperation("Удалить патч-ноут", "")]
	public async Task<IResult> DeletePatchNoteAsync(Guid projectId, Guid patchNoteId)
	    => await Mediator.Send(new P019Request(projectId, patchNoteId));
}
