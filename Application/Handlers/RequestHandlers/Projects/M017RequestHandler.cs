using Application.Contracts.Repository;
using Application.Contracts.Services;
using Ardalis.Specification;
using Domain.Aggregators.Project;
using SharedLibrary.ApiMessages.Projects.M017;
using SharedLibrary.Helpers;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class M017RequestHandler : IRequestHandler<M017Request, IResult<M017Response>>
{
    private readonly IFileStorageService _storageService;
    private readonly IReadRepository<Project> _repository;

    public M017RequestHandler(IFileStorageService storageService, IReadRepository<Project> repository)
    {
        _storageService = storageService;
        _repository = repository;
    }
    public async Task<IResult<M017Response>> Handle(M017Request request, CancellationToken cancellationToken)
    {
        var project = await _repository.SingleOrDefaultAsync(new GetSingleProjectById(request.ProjectId));
        ThrowHelper.NotFoundEntity(project, request.ProjectId.ToString(), nameof(Project));
        var release = project.GetRelease(request.ReleaseId);
        var fileStream = _storageService.DownloadAsync(release.Url);
        return Result<M017Response>.Success(new M017Response(fileStream, "application/octet-stream"));
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
