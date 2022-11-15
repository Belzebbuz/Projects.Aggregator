using MediatR;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.M022;
/// <summary>
/// Get projects names
/// </summary>
/// <param name="text"></param>
public record M022Request(string? text) : IRequest<IResult<List<string>>>;