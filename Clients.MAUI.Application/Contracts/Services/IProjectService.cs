﻿using Clients.MAUI.Application.Contracts.Services.Common;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.ApiMessages.Projects.P003;
using SharedLibrary.ApiMessages.Projects.P004;
using SharedLibrary.ApiMessages.Projects.P012;
using SharedLibrary.Wrapper;

namespace Clients.MAUI.Application.Contracts.Services;

public interface IProjectService : ITransientService
{
    public Task<IResult> CreateProjectAsync(P003Request request);
    public Task<IResult> UpdateProjectAsync(P004Request request);
    public Task<PaginatedResult<ProjectShortDto>> GetAllProjectsAsync(int page, int itemsPerPage);
    public Task<PaginatedResult<ProjectShortDto>> GetProjectsByFilterAsync(P012Request request, int page, int itemsPerPage);
    public Task<IResult<ProjectDto>> GetProjectByIdAsync(Guid projectId);
    public Task<IResult> DeleteProjectAsync(Guid projectId);
    public Task<IResult> UploadReleaseAsync(Guid projectId, string fileName, string filePath, string contentType);
    public Task<IResult> DeleteReleaseAsync(Guid projectId, Guid releaseId);
    public Task<IResult> DownloadReleaseAsync(Guid projectId, Guid releaseId, string fileName, string folderPath);
    public Task<IResult<ICollection<TagDto>>> GetAllTagsAsync();
    public Task<IResult> AddTagToProjectAsync(Guid projectId, Guid tagId);
    public Task<IResult> DeleteTagAsync(Guid projectId, Guid TagId);
    public Task<IResult<List<string>>> GetProjectNames(string text);
    public Task<IResult> AddOrUpdateReleaseNote(Guid projectId, Guid releaseId, string text);
    public Task<IResult<List<TagDto>>> GetTagsByNameAsync(string value);
}
