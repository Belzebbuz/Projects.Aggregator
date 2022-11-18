using MediatR;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.P015;

/// <summary>
/// Get tags by names
/// </summary>
/// <param name="text"></param>
public record P015Request(string? text) : IRequest<IResult<List<TagDto>>>;
