using Clients.MAUI.Application.Contracts.ViewModels;
using Clients.MAUI.Pages.SharedForms.Dialogs;
using SharedLibrary.ApiMessages.Projects.Dto;

namespace Clients.MAUI.Pages.Home;

public partial class Home
{
	private bool _isFilterOpen = false;
	private bool _loaded = false;
	private bool _searchMode = false;
	private List<IFilteredContainer> _projectsRows = new();
	private IFilteredContainer _searchModeFilteredProjects;
	private string[] _startTagFilters = new[] { "", "Desktop" };
	protected override async Task OnInitializedAsync()
	{
		_loaded = false;
		_loaded = await LoadAsync();
		_searchModeFilteredProjects = _filteredProjectsFactory.Create();
		await _searchModeFilteredProjects.FilterBySelfTagsAsync();
	}
	private async Task<bool> LoadAsync()
	{
		try
		{
			foreach (var startFilterTag in _startTagFilters)
			{
				var projectRow = _filteredProjectsFactory.Create();
				if(string.IsNullOrEmpty(startFilterTag))
				{
					await projectRow.FilterBySelfTagsAsync();
				}
				else
				{
					await projectRow.FilterAsync(startFilterTag);
				}
				_projectsRows.Add(projectRow);
			}

			return true;
		}
		catch (Exception ex)
		{
			_snackBar.Add(ex.Message, Severity.Error);
			return false;
		}
	}

	private string _searchString;
	private int _selectedPage = 1;	
	private async Task SearchProjectsAsync(string value)
	{
		_searchMode = true;
		_searchString = value;
		await SearchProjectsBySearchStringValue();
	}

	private async Task SearchProjectsBySearchStringValue(int selectedPage = 1)
	{
		
		try
		{
			_selectedPage = selectedPage;
			await _searchModeFilteredProjects.FilterBySelfTagsAsync(_searchString, selectedPage);
		}
		catch (Exception ex)
		{
			_snackBar.Add(ex.Message, Severity.Error);
		}
	}

	private async Task RemoveTagOnCloseAsync(MudChip chip)
	{
		var tag = (TagDto)chip.Value;
		foreach (var projectRow in _projectsRows)
		{
			await projectRow.RemoveTagFromFilter(tag);
		}
	}
	private async Task RemoveTagFromSearchModeOnCloseAsync(MudChip chip)
	{
		var tag = (TagDto)chip.Value;
		await _searchModeFilteredProjects.RemoveTagFromFilter(tag);
	}
	public async Task OpenSelectTagDiaolgAsync(IFilteredContainer filteredProjects)
	{
		var parameters = new DialogParameters();
		parameters.Add(nameof(SelectTagsDialog.SelectedTags), filteredProjects.FilterTags.Select(x => x.Id).ToList());
		var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
		var dialog = _dialogService.Show<SelectTagsDialog>("Фильтр по тегам", parameters, options);
		var dialogResult = await dialog.Result;
		if (!dialogResult.Cancelled)
		{
			var selectedTags = (IEnumerable<TagDto>)dialogResult.Data;
			await filteredProjects.FilterAsync(selectedTags.ToList());
		}
	}
}