using App.Shared.Wrapper;
using MediatR;

namespace App.Shared.ApiMessages.Projects.M015;

/// <summary>
/// Delete tag
/// </summary>
/// <param name="ProjectId"></param>
/// <param name="TagId"></param>
public record M016Request(Guid ProjectId, Guid TagId) : IRequest<IResult>;
