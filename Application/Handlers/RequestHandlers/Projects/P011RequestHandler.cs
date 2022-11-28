using Application.Contracts.Repository;
using Ardalis.Specification;
using Domain.Aggregators.ProjectAggregate;
using SharedLibrary.ApiMessages.Projects.P011;
using SharedLibrary.Helpers;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class P011RequestHandler : IRequestHandler<P011Request, IResult>
{
    private readonly IRepository<Project> _repository;

    public P011RequestHandler(IRepository<Project> repository) => _repository = repository;
    public async Task<IResult> Handle(P011Request request, CancellationToken cancellationToken)
    {
        var project = await _repository.SingleOrDefaultAsync(new GetProjectById(request.ProjectId));
        ThrowHelper.NotFoundEntity(project, request.ProjectId.ToString(), nameof(Project));
        project.SetReleaseNote(request.ReleaseId, request.ReleaseNote);
        await _repository.SaveChangesAsync();
        return Result.Success();
    }

    private class GetProjectById : Specification<Project>, ISingleResultSpecification<Project>
    {
        public GetProjectById(Guid id) =>
            Query
            .AsSplitQuery()
            .Include(x => x.Releases)
            .Where(x => x.Id == id);
    }
}
