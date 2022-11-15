using MediatR;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.M010;
/// <summary>
/// Request for list of project short dto
/// </summary>
public record M010Request() : IRequest<PaginatedResult<ProjectShortDto>>;
