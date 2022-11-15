using Application.Contracts.Repository;
using Application.Contracts.Services;
using Ardalis.Specification;
using Domain.Aggregators.Project;
using Mapster;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.ApiMessages.Projects.M020;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class M020RequestHandler : IRequestHandler<M020Request, PaginatedResult<ProjectShortDto>>
{
    private readonly IReadRepository<Project> _repository;
    private readonly IPaginationService _paginationService;

    public M020RequestHandler(IReadRepository<Project> repository, IPaginationService paginationService)
    {
        _repository = repository;
        _paginationService = paginationService;
    }

    public async Task<PaginatedResult<ProjectShortDto>> Handle(M020Request request, CancellationToken cancellationToken)
    {
        var projects = await _repository.ListAsync(new GetProjectsByFilter(request));
        if (request.TagIds.Any())
        {
            projects = projects.Where(x => !request.TagIds.Except(x.Tags.Select(t => t.Id)).Any()).ToList();
        }
        var count = projects.Count();
        var pagination = _paginationService.Calculate(count);
        var paginatedProjects = projects.AsQueryable().Paginate(pagination);
        var projectsDto = paginatedProjects.Adapt<List<ProjectShortDto>>();
        return PaginatedResult<ProjectShortDto>.Success(projectsDto, count, pagination.Page, pagination.RecordsPerPage);

    }

    private class GetProjectsByFilter : Specification<Project>
    {
        public GetProjectsByFilter(M020Request filter)
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