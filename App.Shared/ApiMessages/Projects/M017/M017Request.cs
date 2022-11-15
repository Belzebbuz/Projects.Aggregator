using App.Shared.Wrapper;
using MediatR;

namespace App.Shared.ApiMessages.Projects.M017;

/// <summary>
/// Download release
/// </summary>
/// <param name="ProjectId"></param>
public record M017Request(Guid ProjectId, Guid ReleaseId) : IRequest<IResult<M017Response>>;
