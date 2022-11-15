using Application.Contracts.Repository;
using Application.Contracts.Services;
using Ardalis.Specification;
using Domain.Aggregators.Project;
using SharedLibrary.ApiMessages.Projects.M018;
using SharedLibrary.Helpers;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class M018RequestHandler : IRequestHandler<M018Request, IResult>
{
    private readonly IRepository<Project> _repository;
    private readonly IFileStorageService _storageService;

    public M018RequestHandler(IRepository<Project> repository, IFileStorageService storageService)
    {
        _repository = repository;
        _storageService = storageService;
    }
    public async Task<IResult> Handle(M018Request request, CancellationToken cancellationToken)
    {
        var project = await _repository.SingleOrDefaultAsync(new GetProjectById(request.ProjectId));
        ThrowHelper.NotFoundEntity(project, request.ToString(), nameof(Project));
        _storageService.DeleteFiles(project.Releases.Select(x => x.Url));
        await _repository.DeleteAsync(project);
        return Result.Success();
    }

    private class GetProjectById : Specification<Project>, ISingleResultSpecification<Project>
    {
        internal GetProjectById(Guid projectId)
            => Query
            .AsSplitQuery()
            .Where(x => x.Id == projectId)
            .Include(x => x.Releases);

    }
}