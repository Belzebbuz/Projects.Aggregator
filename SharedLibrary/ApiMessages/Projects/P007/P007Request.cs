using MediatR;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.P007;

/// <summary>
/// Add project tag
/// </summary>
/// <param name="projectId"></param>
/// <param name="Text"></param>
public record P007Request(Guid ProjectId, Guid TagId) : IRequest<IResult>;
