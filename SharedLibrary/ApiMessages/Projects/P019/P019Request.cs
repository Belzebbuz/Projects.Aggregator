using MediatR;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.P019;

/// <summary>
/// Delete patch note request
/// </summary>
public record P019Request(Guid ProjectId, Guid PatchNoteId) : IRequest<IResult>;
