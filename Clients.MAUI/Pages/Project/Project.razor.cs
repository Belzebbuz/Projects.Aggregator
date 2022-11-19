using Clients.MAUI.Pages.Project.Releases;
using Clients.MAUI.Pages.SharedForms.Dialogs;
using Clients.MAUI.Utilities;
using Microsoft.AspNetCore.Components;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.ApiMessages.Projects.P020;

namespace Clients.MAUI.Pages.Project;

public partial class Project
{
	private bool _isLoaded = false;
	[Parameter] public string Id { get; set; }
	private ProjectDto _project = new();
	private List<PatchNoteDto> _notes = new ();
	private int _patchNotesSelectedPage = 1;
	private int _patchNotesPerPage = 5;
	private int _patchNotesPageCount;
	protected override async Task OnInitializedAsync()
	{
		_isLoaded = false;
		await LoadAsync();
		await LoadPatchNotesAsync();
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

	private async Task LoadPatchNotesAsync(int selectedPage = 1)
	{
		_patchNotesSelectedPage = selectedPage;
		var notesResult = await _projectService.GetPatchNotesAsync(_project.Id, _patchNotesSelectedPage, _patchNotesPerPage);
		if (notesResult.Data != null)
		{
			_notes = notesResult.Data;
			_patchNotesPageCount = notesResult.TotalPages;
		}
	}

	private async Task AddPatchNoteAsync()
	{
		var parameters = new DialogParameters();
		var dialog = _dialogService.Show<AddPatchNoteDialog>("Добавить описание изменения", parameters, new DialogOptions() { FullWidth = true });
		var dialogResult = await dialog.Result;
		if (!dialogResult.Cancelled)
		{
			var result = await _projectService.AddPatchNoteAsync(new(_project.Id, (string)dialogResult.Data));
			_snackBar.HandleResult(result, "Описание добавлено.");
		}
		await LoadPatchNotesAsync();
	}
	private bool _processingLoadToServer = false;
	private async Task UploadFileAsync()
	{
		_processingLoadToServer = true;
		var selectResult = await SelectZipFileAsync();
		if (selectResult == null)
		{
			_processingLoadToServer = false;
			return;
		}

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

	private async Task DeleteProjectAsync()
	{
		var parameters = new DialogParameters
			{
				{nameof(DeleteConfirmDialog.ContentText), $"Приложение {_project.Name} со всеми релизами." }
			};
		var dialog = _dialogService.Show<DeleteConfirmDialog>("Удаление релиза", parameters, new DialogOptions() { FullWidth = true, CloseButton = true });
		var dialogResult = await dialog.Result;
		if (!dialogResult.Cancelled)
		{
			var deleteResult = await _projectService.DeleteProjectAsync(_project.Id);
			_snackBar.HandleResult(deleteResult, () => _navManager.NavigateTo("/"));
		}
	}
}