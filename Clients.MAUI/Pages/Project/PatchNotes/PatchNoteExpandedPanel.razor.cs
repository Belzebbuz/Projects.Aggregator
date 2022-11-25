using Clients.MAUI.Pages.SharedForms.Dialogs;
using Clients.MAUI.Utilities;
using Microsoft.AspNetCore.Components;
using SharedLibrary.ApiMessages.Projects.Dto;

namespace Clients.MAUI.Pages.Project.PatchNotes;

public partial class PatchNoteExpandedPanel
{
	[Parameter] public PatchNoteDto Note { get; set; }
	[Parameter] public Guid ProjectId { get; set; }
	[Parameter] public EventCallback OnRequestUpdate { get; set; }
	private bool _isOpen;

	private async Task DeletePatchNoteAsync()
	{
		var parameters = new DialogParameters
			{
				{nameof(DeleteConfirmDialog.ContentText), $"Запись от {Note.LastModifiedOn}." }
			};
		var dialog = _dialogService.Show<DeleteConfirmDialog>("Удаление релиза", parameters, new DialogOptions() { FullWidth = true, CloseButton = true });
		var dialogResult = await dialog.Result;
		if (!dialogResult.Cancelled)
		{
			var deleteResult = await _projectService.DeletePatchNoteAsync(ProjectId, Note.Id);
			_snackBar.HandleResult(deleteResult, async () => await OnRequestUpdate.InvokeAsync());
		}

	}
}