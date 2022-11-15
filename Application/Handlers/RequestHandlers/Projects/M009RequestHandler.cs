using Application.Contracts.Repository;
using Ardalis.Specification;
using Domain.Aggregators.Project;
using Mapster;
using Microsoft.Extensions.Localization;
using SharedLibrary.ApiMessages.Projects.M009;
using SharedLibrary.Helpers;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class M009RequestHandler : IRequestHandler<M009Request, IResult<M009Response>>
{
    private readonly IStringLocalizer<M009RequestHandler> _localizer;
    private readonly IReadRepository<Project> _repository;

    public M009RequestHandler(
        IStringLocalizer<M009RequestHandler> localizer,
        IReadRepository<Project> repository)
    {
        _localizer = localizer;
        _repository = repository;
    }
    public async Task<IResult<M009Response>> Handle(M009Request request, CancellationToken cancellationToken)
    {
        var project = await _repository.SingleOrDefaultAsync(new GetProjectById(request.ProjectId));
        ThrowHelper.NotFoundEntity(project, request.ProjectId.ToString(), nameof(Project));

        var dto = project.Adapt<M009Response>();
        return Result<M009Response>.Success(data: dto);
    }

    private class GetProjectById : Specification<Project>, ISingleResultSpecification<Project>
    {
        public GetProjectById(Guid id) =>
            Query
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Releases.OrderByDescending(x => x.LastModifiedOn))
            .Include(x => x.Tags)
            .Where(x => x.Id == id);
    }
}
