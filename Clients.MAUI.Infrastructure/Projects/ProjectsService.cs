using App.Shared.ApiMessages.Projects.M015;
using Clients.MAUI.Application.Contracts.Services;
using Clients.MAUI.Infrastructure.Extensions;
using SharedLibrary;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.ApiMessages.Projects.M011;
using SharedLibrary.ApiMessages.Projects.M012;
using SharedLibrary.ApiMessages.Projects.M019;
using SharedLibrary.ApiMessages.Projects.M020;
using SharedLibrary.ApiMessages.Projects.M021;
using SharedLibrary.ApiMessages.Projects.M024;
using SharedLibrary.ApiMessages.Projects.M025;
using SharedLibrary.Helpers;
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
		var requestResult = await ThrowHelper.TrySendRequest(async () =>
		{
			return await _client.PostAsJsonAsync(ProjectsEndpoints.GetTagsRoute(), new M015Request(projectId, tagId));
		});
		if (!requestResult.Succeeded)
			return requestResult;

		return await requestResult.Data.ToResult();
	}

	public async Task<IResult> CreateProjectAsync(M011Request request)
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
		try
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
		catch (Exception ex)
		{
			return Result.Fail(ex.Message);
		}

	}

	public async Task<PaginatedResult<ProjectShortDto>> GetAllProjectsAsync(int page, int itemsPerPage)
	{
		_client.DefaultRequestHeaders.Add(Headers.Page, page.ToString());
		_client.DefaultRequestHeaders.Add(Headers.ItemsPerPage, itemsPerPage.ToString());

		var response = await _client.GetAsync(ProjectsEndpoints.Base);
		return await response.ToPaginatedResult<ProjectShortDto>();
	}

	public async Task<IResult<ICollection<TagDto>>> GetAllTagsAsync()
	{
		var responseResult = await ThrowHelper.TrySendRequest(async () =>
		{
			return await _client.GetAsync(ProjectsEndpoints.GetTagsRoute());
		});

		if (!responseResult.Succeeded)
			return Result<ICollection<TagDto>>.Fail(responseResult.Messages);

		var result = await responseResult.Data.ToResult<M021Response>();
		if (!result.Succeeded)
			return Result<ICollection<TagDto>>.Fail(result.Messages);

		return Result<ICollection<TagDto>>.Success(result.Data.Tags);
	}

	public async Task<IResult<ProjectDto>> GetProjectByIdAsync(Guid projectId)
	{
		var response = await _client.GetAsync(ProjectsEndpoints.GetProjectRoute(projectId));
		return await response.ToResult<ProjectDto>();
	}

	public async Task<IResult<List<string>>> GetProjectNames(string text)
	{
		try
		{
			var response = await _client.GetAsync(ProjectsEndpoints.GetProjectsNamesRoute(text));
			return await response.ToResult<List<string>>();
		}
		catch (Exception ex)
		{
			return Result<List<string>>.Fail(ex.Message);
		}
	}

	public async Task<PaginatedResult<ProjectShortDto>> GetProjectsByFilterAsync(M020Request request, int page, int itemsPerPage)
	{
		_client.AddOrUpdatePaginationHeaders(page, itemsPerPage);
		var response = await _client.PostAsJsonAsync(ProjectsEndpoints.GetProjectFilterRoute(), request);
		return await response.ToPaginatedResult<ProjectShortDto>();
	}

	public async Task<IResult> UpdateProjectAsync(M012Request request)
	{
		var response = await _client.PutAsJsonAsync(ProjectsEndpoints.Base, request);
		return await response.ToResult();
	}

	public async Task<IResult> AddOrUpdateReleaseNote(Guid projectId, Guid releaseId, string? text)
	{
		var response = await _client.PostAsJsonAsync(ProjectsEndpoints.GetReleaseNoteRoute(), new M019Request(projectId, releaseId, text));
		return await response.ToResult();
	}

	public async Task<IResult<List<TagDto>>> GetTagsByNameAsync(string value)
	{
		var response = await _client.GetAsync(ProjectsEndpoints.GetTagsFilterRoute(value ?? String.Empty));
		return await response.ToResult<List<TagDto>>();
	}

	public async Task<IResult<List<Guid>>> CreateTagsAsync(List<TagDto> tags)
	{
		var response = await _client.PostAsJsonAsync(ProjectsEndpoints.GetMultipleTagsRoute(), new M025Request(tags.Select(x => x.Value).ToList()));
		return await response.ToResult<List<Guid>>();
	}

	public async Task<IResult<Guid>> CreateSingleTagAsync(string value)
	{
		var response = await _client.PostAsJsonAsync(ProjectsEndpoints.GetTagsRoute(), new M024Request(value));
		return await response.ToResult<Guid>();
	}
}
