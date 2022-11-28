using Application.Contracts.Repository;
using Application.Contracts.Services;
using Ardalis.Specification;
using Domain.Aggregators.ProjectAggregate;
using SharedLibrary.ApiMessages.Projects.P009;
using SharedLibrary.Helpers;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class P009RequestHandler : IRequestHandler<P009Request, IResult<P009Response>>
{
    private readonly IFileStorageService _storageService;
    private readonly IReadRepository<Project> _repository;

    public P009RequestHandler(IFileStorageService storageService, IReadRepository<Project> repository)
    {
        _storageService = storageService;
        _repository = repository;
    }
    public async Task<IResult<P009Response>> Handle(P009Request request, CancellationToken cancellationToken)
    {
        var project = await _repository.SingleOrDefaultAsync(new GetSingleProjectById(request.ProjectId));
        ThrowHelper.NotFoundEntity(project, request.ProjectId.ToString(), nameof(Project));
        var release = project.GetRelease(request.ReleaseId);
        var fileStreamResult = _storageService.DownloadAsync(release.Url);
        return Result<P009Response>.Success(new P009Response(fileStreamResult, "application/octet-stream"));
    }

    private class GetSingleProjectById : Specification<Project>, ISingleResultSpecification<Project>
    {
        internal GetSingleProjectById(Guid Id)
            => Query
            .AsSplitQuery()
            .Include(x => x.Releases)
            .Where(x => x.Id == Id);
    }
}
