using Clients.MAUI.Utilities;
using SharedLibrary.ApiMessages.Projects.Dto;

namespace Clients.MAUI.Pages.Home;

public partial class Home
{
	private string _selectedSearchValue;
	private bool _loaded = false;
	private bool _searchMode = false;
	private List<ProjectShortDto> _webProjects = new();
	private List<ProjectShortDto> _desktopProjects = new();
	private List<ProjectShortDto> _searchAllProjects = new();
	protected override async Task OnInitializedAsync()
	{
		_loaded = false;
		await LoadAsync();
		_loaded = true;
	}

	private async Task LoadAsync()
	{
		var tagsResult = await _projectService.GetAllTagsAsync();
		var tags = _snackBar.HandleResult(tagsResult);
		if (tags == null)
			return;

		var webIds = tags.Where(x => x.Value == "Web").Select(x => x.Id).ToList();
		var desktopIds = tagsResult.Data.Where(x => x.Value == "Desktop").Select(x => x.Id).ToList();
		var webProjectsResult = await _projectService.GetProjectsByFilterAsync(new(string.Empty, webIds), 1, 5);
		var webProjects = _snackBar.HandleResult(webProjectsResult);
		if (webProjects == null)
			return;

		_webProjects = webProjects.Data;
		var projectsResult = await _projectService.GetProjectsByFilterAsync(new(string.Empty, desktopIds), 1, 5);
		var projects = _snackBar.HandleResult(projectsResult);
		if(projects == null)
			return;
		_desktopProjects = projects.Data;
	}

	private int _pageCount = 1;
	private string _searchString;
	private async Task<IEnumerable<string>> SearchProjectsAsync(string value)
	{
		_searchMode = true;
		if (!string.IsNullOrEmpty(value))
		{
			_searchString = value;
			await SearchProjectsBySearchStringValue();
		}
		else
		{
			_searchString = string.Empty;
			_searchMode = false;
		}
		return new List<string>();
	}

	private async Task SearchProjectsBySearchStringValue(int selectedPage = 1)
	{
		var projectsResult = await _projectService.GetProjectsByFilterAsync(new(_searchString), selectedPage, 5);
		var projectShortPaginate = _snackBar.HandleResult(projectsResult);
		if(projectShortPaginate == null)
			return;
		_pageCount = projectShortPaginate.TotalPages;
		_searchAllProjects = projectShortPaginate.Data;
	}
}