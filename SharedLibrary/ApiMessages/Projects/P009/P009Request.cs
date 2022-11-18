using MediatR;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.P009;

/// <summary>
/// Download release
/// </summary>
/// <param name="ProjectId"></param>
public record P009Request(Guid ProjectId, Guid ReleaseId) : IRequest<IResult<P009Response>>;
