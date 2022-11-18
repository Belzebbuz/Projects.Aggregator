using MediatR;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.P001;

/// <summary>
/// GET project by Id
/// </summary>
/// <param name="ProjectId"></param>
public record P001Request(Guid ProjectId) : IRequest<IResult<P001Response>>;
