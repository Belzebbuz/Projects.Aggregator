using App.Shared.ApiMessages.Projects.Dto;
using App.Shared.Wrapper;
using MediatR;

namespace App.Shared.ApiMessages.Projects.M010;
/// <summary>
/// Request for list of project short dto
/// </summary>
public record M010Request() : IRequest<PaginatedResult<ProjectShortDto>>;
