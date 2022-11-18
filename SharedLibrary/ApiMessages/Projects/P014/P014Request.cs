using MediatR;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.P014;
/// <summary>
/// Get projects names
/// </summary>
/// <param name="text"></param>
public record P014Request(string? text) : IRequest<IResult<List<string>>>;