using MediatR;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.M009;

/// <summary>
/// GET project by Id
/// </summary>
/// <param name="ProjectId"></param>
public record M009Request(Guid ProjectId) : IRequest<IResult<M009Response>>;
