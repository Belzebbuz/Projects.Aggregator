using MediatR;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.P010;

/// <summary>
/// Delete project
/// </summary>
/// <param name="projectId"></param>
public record P010Request(Guid ProjectId) : IRequest<IResult>;
