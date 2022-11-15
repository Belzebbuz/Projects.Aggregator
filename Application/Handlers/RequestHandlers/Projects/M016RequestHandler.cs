using Application.Contracts.Repository;
using Ardalis.Specification;
using Domain.Aggregators.Project;
using SharedLibrary.Helpers;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class M016RequestHandler : IRequestHandler<M016Request, IResult>
{
    private readonly IRepository<Project> _repository;

    public M016RequestHandler(IRepository<Project> repository)
    {
        _repository = repository;
    }
    public async Task<IResult> Handle(M016Request request, CancellationToken cancellationToken)
    {
        var project = await _repository.SingleOrDefaultAsync(new GetProjectById(request.ProjectId));
        ThrowHelper.NotFoundEntity(project, request.ProjectId.ToString(), nameof(Project));
        project.RemoveTag(request.TagId);
        await _repository.SaveChangesAsync();
        return Result.Success();
    }

    private class GetProjectById : Specification<Project>, ISingleResultSpecification<Project>
    {
        internal GetProjectById(Guid projectId)
            => Query
            .AsSplitQuery()
            .Where(x => x.Id == projectId)
            .Include(x => x.Releases)
            .Include(x => x.Tags);

    }
}
