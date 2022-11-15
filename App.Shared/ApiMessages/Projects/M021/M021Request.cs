using App.Shared.Wrapper;
using MediatR;

namespace App.Shared.ApiMessages.Projects.M021;

/// <summary>
/// Get all tags request
/// </summary>
public record M021Request() : IRequest<IResult<M021Response>>;
