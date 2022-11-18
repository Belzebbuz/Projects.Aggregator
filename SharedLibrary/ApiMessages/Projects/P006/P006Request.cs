using MediatR;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.P006;

/// <summary>
/// Delete release
/// </summary>
/// <param name="id"></param>
public record P006Request(Guid ProjectId, Guid ReleaseId) : IRequest<IResult>;
