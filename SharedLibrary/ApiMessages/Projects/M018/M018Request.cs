using MediatR;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.M018;

/// <summary>
/// Delete project
/// </summary>
/// <param name="projectId"></param>
public record M018Request(Guid ProjectId) : IRequest<IResult>;
