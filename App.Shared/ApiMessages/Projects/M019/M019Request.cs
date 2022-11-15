using App.Shared.Wrapper;
using MediatR;

namespace App.Shared.ApiMessages.Projects.M019;

/// <summary>
/// Add or update release note request
/// </summary>
/// <param name="ProjectId"></param>
/// <param name="ReleaseId"></param>
/// <param name="ReleaseNote"></param>
public record M019Request(Guid ProjectId, Guid ReleaseId, string ReleaseNote) : IRequest<IResult>;
