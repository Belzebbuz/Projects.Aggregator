using Application.Contracts.Repository;
using Ardalis.Specification;
using Domain.Aggregators.Project;
using Mapster;
using Microsoft.Extensions.Localization;
using SharedLibrary.ApiMessages.Projects.P001;
using SharedLibrary.Helpers;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class P001RequestHandler : IRequestHandler<P001Request, IResult<P001Response>>
{
    private readonly IStringLocalizer<P001RequestHandler> _localizer;
    private readonly IReadRepository<Project> _repository;

    public P001RequestHandler(
        IStringLocalizer<P001RequestHandler> localizer,
        IReadRepository<Project> repository)
    {
        _localizer = localizer;
        _repository = repository;
    }
    public async Task<IResult<P001Response>> Handle(P001Request request, CancellationToken cancellationToken)
    {
        var project = await _repository.SingleOrDefaultAsync(new GetProjectById(request.ProjectId));
        ThrowHelper.NotFoundEntity(project, request.ProjectId.ToString(), nameof(Project));

        var dto = project.Adapt<P001Response>();
        return Result<P001Response>.Success(data: dto);
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
