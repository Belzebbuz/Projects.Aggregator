using App.Shared.Wrapper;
using MediatR;

namespace App.Shared.ApiMessages.Projects.M015;

/// <summary>
/// Add project tag
/// </summary>
/// <param name="projectId"></param>
/// <param name="Text"></param>
public record M015Request(Guid ProjectId, Guid TagId) : IRequest<IResult>;
