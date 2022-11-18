using Clients.MAUI.Pages.SharedForms.Dialogs;
using Clients.MAUI.Utilities;
using Microsoft.AspNetCore.Components;
using SharedLibrary.ApiMessages.Projects.Dto;

namespace Clients.MAUI.Pages.Project.Releases;

public partial class ReleaseExpandedPanel
{
	[Parameter]
	public Guid ProjectId { get; set; }
	[Parameter, EditorRequired]
	public string DownloadFileName { get; set; }
	[Parameter]
	public ReleaseDto Release { get; set; }

	[Parameter] public EventCallback OnRequestUpdate { get; set; }
	private bool _isOpen;
	private bool _downloading = false;
	private async Task DownloadReleaseAsync()
	{
		_downloading = true;
		var folder = await _folderPicker.PickFolder();
		if (folder == null)
		{
			_downloading = false;
			return;
		}
		await Task.Delay(2000);
		var downloadResult = await _projectService.DownloadReleaseAsync(ProjectId, Release.Id, DownloadFileName, folder);
		_snackBar.HandleResult(downloadResult, "Релиз скачан.");
		_downloading = false;
	}

	public async Task AddOrUpdateReleaseNoteAsync()
	{
		var parameters = new DialogParameters
			{
				{nameof(AddOrUpdateReleaseNoteDialog.ReleaseNoteValue), Release.ReleaseNote }
			};
		var dialog = _dialogService.Show<AddOrUpdateReleaseNoteDialog>("Добавление описания", parameters, new DialogOptions() { FullWidth = true });
		var dialogResult = await dialog.Result;
		if (!dialogResult.Cancelled)
		{
			var result = await _projectService.AddOrUpdateReleaseNote(ProjectId, Release.Id, (string)dialogResult.Data);
			_snackBar.HandleResult(result, "Описание добавлено.");
		}
		await OnRequestUpdate.InvokeAsync();
	}

	public async Task DeleteReleaseAsync()
	{
		var parameters = new DialogParameters
			{
				{nameof(DeleteConfirmDialog.ContentText), $"Релиз от {Release.LastModifiedOn} v{Release.Version}." }
			};
		var dialog = _dialogService.Show<DeleteConfirmDialog>("Удаление релиза", parameters, new DialogOptions() { FullWidth = true , CloseButton = true});
		var dialogResult = await dialog.Result;
		if (!dialogResult.Cancelled)
		{
			var deleteResult = await _projectService.DeleteReleaseAsync(ProjectId, Release.Id);
			_snackBar.HandleResult(deleteResult, async () => await OnRequestUpdate.InvokeAsync());
		}
	}

}