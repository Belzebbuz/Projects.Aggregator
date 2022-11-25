using Application.Contracts.Repository;
using Application.Contracts.Services;
using Application.Options;
using Ardalis.Specification;
using Domain.Aggregators.Project;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedLibrary;
using SharedLibrary.ApiMessages.Projects.P005;
using SharedLibrary.Helpers;
using SharedLibrary.Wrapper;
using IResult = SharedLibrary.Wrapper.IResult;

namespace Application.Handlers.RequestHandlers.Projects;

public class P005RequestHandler : IRequestHandler<P005Request, IResult>
{
	private readonly IFileStorageService _storageService;
	private readonly IRepository<Project> _repository;
	private readonly ILogger<P005RequestHandler> _logger;
	private readonly ReleaseOptions _releaseOptions;
	private readonly HttpRequest _request;

	public P005RequestHandler(IFileStorageService storageService, 
		IRepository<Project> repository, 
		IOptions<ReleaseOptions> options, 
		IHttpContextAccessor httpContextAccessor, ILogger<P005RequestHandler> logger)
	{
		_storageService = storageService;
		_repository = repository;
		_logger = logger;
		_releaseOptions = options.Value;
		_request = httpContextAccessor.HttpContext.Request;
	}
	public async Task<IResult> Handle(P005Request request, CancellationToken cancellationToken)
	{
		var projectId = _request.Headers[Headers.ProjectId];
		if ((string)projectId == null)
			return Result.Fail();
		var project = await _repository.SingleOrDefaultAsync(new GetProjectById(Guid.Parse(projectId)));
		ThrowHelper.NotFoundEntity(project, (string)projectId, nameof(Project));

		var result = await _storageService.SaveFileStreamingAsync((string)projectId);
		if (!result.Succeeded)
			return Result.Fail(result.Messages);

		var exeFileInfoResult = await _storageService.GetExeFileVersionAsync(result.Data, project.ExeFileName);
		if (!exeFileInfoResult.Succeeded)
		{
			_storageService.DeleteSingleFile(result.Data);
			return Result.Fail(exeFileInfoResult.Messages);
		}

		var createdRelease = project.AddRelease(exeFileInfoResult.Data.Version ?? "1.0.0",
			result.Data, exeFileInfoResult.Data.GitSha, exeFileInfoResult.Data.GitBranch, true);

		ClearOldReleases(project);

		await _repository.UpdateAsync(project);
		return Result.Success(createdRelease.Data.Id.ToString());
	}

	private void ClearOldReleases(Project project)
	{
		if (project.Releases.Count >= _releaseOptions.SaveLastReleases)
		{
			var lastReleases = project.GetLastReleases(_releaseOptions.SaveLastReleases);
			foreach (var release in lastReleases)
			{
				_storageService.DeleteFiles(lastReleases.Select(x => x.Url));
			}
			project.RemoveReleases(lastReleases);
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
