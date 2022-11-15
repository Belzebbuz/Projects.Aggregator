using Clients.MAUI.Utilities;
using Microsoft.AspNetCore.Components;
using SharedLibrary.ApiMessages.Projects.Dto;

namespace Clients.MAUI.Pages.Project;

public partial class Project
{
	private bool _isLoaded = false;
	[Parameter] public string Id { get; set; }
	private ProjectDto _project = new();
	protected override async Task OnInitializedAsync()
	{
		_isLoaded = false;
		await LoadAsync();
		_isLoaded = true;
	}

	private async Task LoadAsync()
	{
		var projectResult = await _projectService.GetProjectByIdAsync(Guid.Parse(Id));
		var project = _snackBar.HandleResult(projectResult);
		if (project == null)
		{
			_navManager.NavigateTo("/");
			_isLoaded = false;
		}
		_project = project;
	}

	private bool _processingLoadToServer = false;
	private async Task UploadFileAsync()
	{
		var selectResult = await SelectZipFileAsync();
		if (selectResult == null)
			return;

		_processingLoadToServer = true;
		var uploadResult = await _projectService.UploadReleaseAsync(_project.Id, selectResult.FileName,
			selectResult.FullPath, selectResult.ContentType);

		_snackBar.HandleResult(uploadResult, "Релиз добавлен.");
		await OnInitializedAsync();
		_processingLoadToServer = false;
	}

	private async Task<FileResult> SelectZipFileAsync()
	{
		_processingLoadToServer = true;
		var customFileType = new FilePickerFileType(
				new Dictionary<DevicePlatform, IEnumerable<string>>
					{{ DevicePlatform.WinUI, new[] { ".zip" } }});
		PickOptions options = new()
		{
			PickerTitle = "Выберите архив с собранным проектом",
			FileTypes = customFileType,
		};
		return await _filePicker.PickAsync(options);
	}
}