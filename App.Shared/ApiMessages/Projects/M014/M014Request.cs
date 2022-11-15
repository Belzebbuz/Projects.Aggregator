using App.Shared.Wrapper;
using MediatR;

namespace App.Shared.ApiMessages.Projects.M014;

/// <summary>
/// Delete release
/// </summary>
/// <param name="id"></param>
public record M014Request(Guid ProjectId, Guid ReleaseId) : IRequest<IResult>;
