using MediatR;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.M014;

/// <summary>
/// Delete release
/// </summary>
/// <param name="id"></param>
public record M014Request(Guid ProjectId, Guid ReleaseId) : IRequest<IResult>;
