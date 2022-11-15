using MediatR;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.M023;

/// <summary>
/// Get tags by names
/// </summary>
/// <param name="text"></param>
public record M023Request(string? text) : IRequest<IResult<List<TagDto>>>;
