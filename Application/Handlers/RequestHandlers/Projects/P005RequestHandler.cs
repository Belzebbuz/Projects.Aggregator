using Application.Contracts.Repository;
using Application.Contracts.Services;
using Application.Options;
using Ardalis.Specification;
using Domain.Aggregators.Project;
using Microsoft.Extensions.Options;
using SharedLibrary.ApiMessages.Projects.P005;
using SharedLibrary.Helpers;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class P005RequestHandler : IRequestHandler<P005Request, IResult>
{
    private readonly IFileStorageService _storageService;
    private readonly IRepository<Project> _repository;
    private readonly ReleaseOptions _releaseOptions;

    public P005RequestHandler(IFileStorageService storageService, IRepository<Project> repository, IOptions<ReleaseOptions> options)
    {
        _storageService = storageService;
        _repository = repository;
        _releaseOptions = options.Value;
    }
    public async Task<IResult> Handle(P005Request request, CancellationToken cancellationToken)
    {
        try
        {
            var project = await _repository.SingleOrDefaultAsync(new GetProjectById(request.ProjectId));
            ThrowHelper.NotFoundEntity(project, request.ProjectId.ToString(), nameof(Project));

            var result = await _storageService.UploadZipProjectAsync(project.Id, project.Name, request.FileName, project.ExeFileName, request.FileStream);
            if (!result.Succeeded)
                return Result.Fail(result.Messages);
            var createdRelease = project.AddRelease(result.Data.Version, result.Data.Url, result.Data.GitSha, result.Data.GitBranch, true);

            if (project.Releases.Count >= _releaseOptions.SaveLastReleases)
            {
                var lastReleases = project.GetLastReleases(_releaseOptions.SaveLastReleases);
                foreach (var release in lastReleases)
                {
                    _storageService.DeleteFiles(lastReleases.Select(x => x.Url));
                }
                project.RemoveReleases(lastReleases);
            }

            await _repository.UpdateAsync(project);
            return Result.Success(createdRelease.Data.Id.ToString());
        }
        finally
        {
            request.FileStream?.Dispose();
        }
    }

    private class GetProjectById : Specification<Project>, ISingleResultSpecification<Project>
    {
        public GetProjectById(Guid projectId)
            => Query
            .Include(x => x.Releases)
            .Where(x => x.Id == projectId);
    }
}
