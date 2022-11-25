using MediatR;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.P005;

/// <summary>
/// Add release file
/// </summary>
/// <param name="id"></param>
public record P005Request() : IRequest<IResult>;
