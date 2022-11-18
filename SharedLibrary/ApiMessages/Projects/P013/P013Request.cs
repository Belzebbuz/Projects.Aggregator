using MediatR;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.P013;

/// <summary>
/// Get all tags request
/// </summary>
public record P013Request() : IRequest<IResult<P013Response>>;
