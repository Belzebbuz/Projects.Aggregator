using Application.Contracts.Repository;
using Application.Contracts.Services;
using Ardalis.Specification;
using Domain.Aggregators.ProjectAggregate;
using Mapster;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.ApiMessages.Projects.P012;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class P012RequestHandler : IRequestHandler<P012Request, PaginatedResult<ProjectShortDto>>
{
    private readonly IReadRepository<Project> _repository;
    private readonly IPaginationService _paginationService;

    public P012RequestHandler(IReadRepository<Project> repository, IPaginationService paginationService)
    {
        _repository = repository;
        _paginationService = paginationService;
    }

    public async Task<PaginatedResult<ProjectShortDto>> Handle(P012Request request, CancellationToken cancellationToken)
    {
        var projects = await _repository.ListAsync(new GetProjectsByFilter(request));
        if (request.TagIds.Any())
        {
            projects = projects.Where(x => !request.TagIds.Except(x.Tags.Select(t => t.Id)).Any()).ToList();
        }
        var paginatedProjects = projects.AsQueryable().Paginate(_paginationService.Pagination);
        var projectsDto = paginatedProjects.Adapt<List<ProjectShortDto>>();
        return PaginatedResult<ProjectShortDto>.Success(projectsDto, 
            _paginationService.Pagination.ItemsCount, 
            _paginationService.Pagination.Page, 
            _paginationService.Pagination.RecordsPerPage);

    }

    private class GetProjectsByFilter : Specification<Project>
    {
        public GetProjectsByFilter(P012Request filter)
        {
            Query
                .AsNoTracking()
                .OrderByDescending(x => x.LastModifiedOn)
                .Include(x => x.Tags);

            if (!string.IsNullOrWhiteSpace(filter.FilterName))
                Query
                    .Where(x => x.Name.ToLower().Contains(filter.FilterName.ToLower()));
        }
    }

}