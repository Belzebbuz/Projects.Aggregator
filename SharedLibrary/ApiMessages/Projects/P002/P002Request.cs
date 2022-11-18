using MediatR;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.P002;
/// <summary>
/// Request for list of project short dto
/// </summary>
public record P002Request() : IRequest<PaginatedResult<ProjectShortDto>>;
