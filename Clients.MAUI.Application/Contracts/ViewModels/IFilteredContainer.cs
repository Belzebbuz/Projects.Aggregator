using SharedLibrary.ApiMessages.Projects.Dto;

namespace Clients.MAUI.Application.Contracts.ViewModels;

public interface IFilteredContainer
{
	public string SearchName { get; }
	public IReadOnlyCollection<TagDto> FilterTags { get; }
	public IReadOnlyCollection<ProjectShortDto> Projects { get; }
	public int Page { get; }
	public int ItemsPerPage { get; }
	public int PageCount { get; }
	public Task FilterAsync(List<TagDto> filterTag, string name = "", int page = 1, int itemsPerPage = 5);
	public Task FilterAsync(string tagName, string name = "", int page = 1, int itemsPerPage = 5);
	public Task FilterAsync(List<string> tagName, string name = "", int page = 1, int itemsPerPage = 5);
    Task FilterBySelfTagsAsync(string name = "", int page = 1, int itemsPerPage = 5);
    public Task RemoveTagFromFilter(TagDto tag);
}