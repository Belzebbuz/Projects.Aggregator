using App.Shared.ApiMessages.Projects.P007;
using Clients.MAUI.Application.Contracts.Services;
using Clients.MAUI.Infrastructure.Extensions;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.ApiMessages.Projects.P003;
using SharedLibrary.ApiMessages.Projects.P004;
using SharedLibrary.ApiMessages.Projects.P011;
using SharedLibrary.ApiMessages.Projects.P012;
using SharedLibrary.ApiMessages.Projects.P013;
using SharedLibrary.Routes;
using SharedLibrary.Wrapper;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Clients.MAUI.Infrastructure.Projects;

public class ProjectsService : IProjectService
{
	private readonly HttpClient _client;

	public ProjectsService(HttpClient client)
	{
		_client = client;
	}
	public async Task<IResult> UploadReleaseAsync(Guid projectId, string fileName, string filePath, string contentType)
	{
		using var multipartFormContent = new MultipartFormDataContent();
		var fileStreamContent = new StreamContent(File.OpenRead(filePath));
		fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
		multipartFormContent.Add(fileStreamContent, name: "file", fileName: fileName);
		var response = await _client.PostAsync(ProjectsEndpoints.GetReleasesRoute(projectId), multipartFormContent);
		return await response.ToResult();
	}

	public async Task<IResult> AddTagToProjectAsync(Guid projectId, Guid tagId)
	{
		var requestResult = await _client.PostAsJsonAsync(ProjectsEndpoints.GetTagsRoute(), new P007Request(projectId, tagId));
		return await requestResult.ToResult();
	}

	public async Task<IResult> CreateProjectAsync(P003Request request)
	{
		var response = await _client.PostAsJsonAsync(ProjectsEndpoints.Base, request);
		return await response.ToResult();
	}

	public async Task<IResult> DeleteProjectAsync(Guid projectId)
	{
		var response = await _client.DeleteAsync(ProjectsEndpoints.GetProjectRoute(projectId));
		return await response.ToResult();
	}

	public async Task<IResult> DeleteReleaseAsync(Guid projectId, Guid releaseId)
	{
		var response = await _client.DeleteAsync(ProjectsEndpoints.GetSingleReleaseRoute(projectId, releaseId));
		return await response.ToResult();
	}

	public async Task<IResult> DeleteTagAsync(Guid projectId, Guid TagId)
	{
		var response = await _client.DeleteAsync(ProjectsEndpoints.GetSingleProjectTagRoute(projectId, TagId));
		return await response.ToResult();
	}

	public async Task<IResult> DownloadReleaseAsync(Guid projectId, Guid releaseId, string fileName, string folderPath)
	{
		var downloadRequest = await _client.GetStreamAsync(ProjectsEndpoints.GetSingleReleaseRoute(projectId, releaseId));
		if (!Directory.Exists(folderPath))
			Directory.CreateDirectory(folderPath);
		var downloadFileName = Path.Combine(folderPath, $"{fileName}.zip");
		await using (var fs = File.OpenWrite(downloadFileName))
		{
			await downloadRequest.CopyToAsync(fs);
		}
		return Result.Success();
	}

	public async Task<PaginatedResult<ProjectShortDto>> GetAllProjectsAsync(int page, int itemsPerPage)
	{
		_client.AddOrUpdatePaginationHeaders(page, itemsPerPage);

		var response = await _client.GetAsync(ProjectsEndpoints.Base);
		return await response.ToPaginatedResult<ProjectShortDto>();
	}

	public async Task<IResult<ICollection<TagDto>>> GetAllTagsAsync()
	{
		var responseResult = await _client.GetAsync(ProjectsEndpoints.GetTagsRoute());

		var result = await responseResult.ToResult<P013Response>();
		if (!result.Succeeded)
			Result<ICollection<TagDto>>.Fail(result.Messages);

		return Result<ICollection<TagDto>>.Success(result.Data.Tags);
	}

	public async Task<IResult<ProjectDto>> GetProjectByIdAsync(Guid projectId)
	{
		var response = await _client.GetAsync(ProjectsEndpoints.GetProjectRoute(projectId));
		return await response.ToResult<ProjectDto>();
	}

	public async Task<IResult<List<string>>> GetProjectNames(string text)
	{
		var response = await _client.GetAsync(ProjectsEndpoints.GetProjectsNamesRoute(text));
		return await response.ToResult<List<string>>();
	}

	public async Task<PaginatedResult<ProjectShortDto>> GetProjectsByFilterAsync(P012Request request, int page, int itemsPerPage)
	{
		_client.AddOrUpdatePaginationHeaders(page, itemsPerPage);
		var response = await _client.PostAsJsonAsync(ProjectsEndpoints.GetProjectFilterRoute(), request);
		return await response.ToPaginatedResult<ProjectShortDto>();
	}

	public async Task<IResult> UpdateProjectAsync(P004Request request)
	{
		var response = await _client.PutAsJsonAsync(ProjectsEndpoints.Base, request);
		return await response.ToResult();
	}

	public async Task<IResult> AddOrUpdateReleaseNote(Guid projectId, Guid releaseId, string? text)
	{
		var response = await _client.PostAsJsonAsync(ProjectsEndpoints.GetReleaseNoteRoute(), new P011Request(projectId, releaseId, text));
		return await response.ToResult();
	}

	public async Task<IResult<List<TagDto>>> GetTagsByNameAsync(string value)
	{
		var response = await _client.GetAsync(ProjectsEndpoints.GetTagsFilterRoute(value ?? String.Empty));
		if (string.IsNullOrEmpty(value))
		{
			var result = await response.ToResult<P013Response>();
			return Result<List<TagDto>>.Success(result.Data.Tags.ToList());
		}
		return await response.ToResult<List<TagDto>>();
	}
}
