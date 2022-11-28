using Application.Contracts.Repository;
using Application.Contracts.Services;
using Ardalis.Specification;
using Domain.Aggregators.ProjectAggregate;
using Mapster;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.ApiMessages.Projects.P002;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class P002RequestHandler : IRequestHandler<P002Request, PaginatedResult<ProjectShortDto>>
{
    private readonly IPaginationService _paginationService;
    private readonly IReadRepository<Project> _repository;

    public P002RequestHandler(
        IPaginationService paginationService,
        IReadRepository<Project> repository)
    {
        _paginationService = paginationService;
        _repository = repository;
    }
    public async Task<PaginatedResult<ProjectShortDto>> Handle(P002Request request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountAsync();
        var pagination = _paginationService.Calculate(count);
        var projects = await _repository.ListAsync(new GetProjects(pagination));
        var projectsDto = projects.Adapt<List<ProjectShortDto>>();
        return PaginatedResult<ProjectShortDto>.Success(projectsDto, count, pagination.Page, pagination.RecordsPerPage);
    }

    private sealed class GetProjects : Specification<Project>
    {
        public GetProjects(Pagination pagination) =>
            Query
            .AsNoTracking()
            .OrderByDescending(x => x.LastModifiedOn)
            .Include(x => x.Tags)
            .Skip((pagination.Page - 1) * pagination.RecordsPerPage)
            .Take(pagination.RecordsPerPage);
    }
}
