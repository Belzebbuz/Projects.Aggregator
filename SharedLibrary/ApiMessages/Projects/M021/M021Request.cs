using MediatR;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.M021;

/// <summary>
/// Get all tags request
/// </summary>
public record M021Request() : IRequest<IResult<M021Response>>;
