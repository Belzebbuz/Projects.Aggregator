using System.Runtime.InteropServices;
using App.Shared.Wrapper;
using MediatR;

namespace App.Shared.ApiMessages.Projects.M009;

/// <summary>
/// GET project by ProjectId
/// </summary>
/// <param name="ProjectId"></param>
public record M009Request(Guid ProjectId): IRequest<IResult<M009Response>>;
