using MediatR;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.P020;

public class P020Request : IRequest<PaginatedResult<PatchNoteDto>>
{
	public Guid ProjectId { get; set; }
}
