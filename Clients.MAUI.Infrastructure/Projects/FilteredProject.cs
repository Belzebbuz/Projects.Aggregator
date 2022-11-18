using Clients.MAUI.Application.Contracts.Services;
using Clients.MAUI.Application.Contracts.ViewModels;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.Wrapper;

namespace Clients.MAUI.Infrastructure.Projects;

public class FilteredProjects : IFilteredContainer
{
    private string _filterName = string.Empty;
    private HashSet<TagDto> _filterTags = new();
    private List<ProjectShortDto> _projects = new();
    private int _page = 1;
    private int _itemsPerPage = 5;
    private int _pageCount;
    private readonly IProjectService _projectService;


    public string SearchName => _filterName;
    public IReadOnlyCollection<TagDto> FilterTags => _filterTags;
    public IReadOnlyCollection<ProjectShortDto> Projects => _projects;
    public int Page => _page;
    public int ItemsPerPage => _itemsPerPage;
    public int PageCount => _pageCount;

    public FilteredProjects(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public async Task FilterBySelfTagsAsync(string name = "", int page = 1, int itemsPerPage = 5)
    {
        await FilterAsync(_filterTags.ToList(), name, page, itemsPerPage);
    }
    public async Task FilterAsync(List<TagDto> filterTags, string name = "", int page = 1, int itemsPerPage = 5)
    {
        var resultProjectDto = await _projectService.GetProjectsByFilterAsync(new(name, filterTags.Select(x => x.Id).ToList()), page, itemsPerPage);
        UpdateState(name, page, itemsPerPage, filterTags, resultProjectDto);
    }

    public async Task FilterAsync(string tagName, string name = "", int page = 1, int itemsPerPage = 5)
    {
        var filterTags = await GetTagsByNameAsync(tagName);
        var resultProjectDto = await _projectService.GetProjectsByFilterAsync(new(name, filterTags.Select(x => x.Id).ToList()), page, itemsPerPage);
        UpdateState(name, page, itemsPerPage, filterTags, resultProjectDto);
    }

    public async Task FilterAsync(List<string> tagName, string name = "", int page = 1, int itemsPerPage = 5)
    {
        var filterTags = new List<TagDto>();
        foreach (var tag in tagName)
        {
            filterTags.AddRange(await GetTagsByNameAsync(tag));
        }
        var resultProjectDto = await _projectService.GetProjectsByFilterAsync(new(name, filterTags.Select(x => x.Id).ToList()), page, itemsPerPage);
        UpdateState(name, page, itemsPerPage, filterTags, resultProjectDto);
    }

    private async Task<List<TagDto>> GetTagsByNameAsync(string name)
    {
        var webTagsResult = await _projectService.GetTagsByNameAsync(name);
        return webTagsResult.Data;
    }
    public async Task RemoveTagFromFilter(TagDto tag)
    {
        if (!_filterTags.Contains(tag))
            return;

        _filterTags.Remove(tag);
        await FilterAsync(_filterTags.ToList(), _filterName, _page, _itemsPerPage);
    }
    private void UpdateState(string name, int page, int itemsPerPage, List<TagDto> filterTags, PaginatedResult<ProjectShortDto> resultProjectDto)
    {
        _projects = resultProjectDto.Data;
        _filterTags = filterTags.ToHashSet();
        _filterName = name;
        _page = page;
        _itemsPerPage = itemsPerPage;
        _pageCount = resultProjectDto.TotalPages;
    }


}
