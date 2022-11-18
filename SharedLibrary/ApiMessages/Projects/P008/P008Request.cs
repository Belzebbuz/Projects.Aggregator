using MediatR;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.P008;

/// <summary>
/// Delete tag
/// </summary>
/// <param name="ProjectId"></param>
/// <param name="TagId"></param>
public record P008Request(Guid ProjectId, Guid TagId) : IRequest<IResult>;
