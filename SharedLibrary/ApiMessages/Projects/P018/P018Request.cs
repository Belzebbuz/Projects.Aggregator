using MediatR;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.P018;

/// <summary>
/// Update patch note request
/// </summary>
public class P018Request : IRequest<IResult>
{
	public Guid ProjectId { get; set; }
	public Guid PatchNoteId { get; set; }
	public string Text { get; set; }
}
