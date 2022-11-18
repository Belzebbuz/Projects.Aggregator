using Mapster;
using Microsoft.AspNetCore.Components;
using SharedLibrary.ApiMessages.Projects.P004;
using SharedLibrary.ApiMessages.Projects.P012;

namespace Clients.MAUI.Pages.Project.UpdatePage;

public partial class UpdateProject
{
	[Parameter] public string ProjectId { get; set; }
	private P004Request _updateRequest = new();
	private bool _loaded = false;
	protected override async Task OnInitializedAsync()
	{
		_loaded = false;
		var project = await _projectService.GetProjectByIdAsync(Guid.Parse(ProjectId));
		if(project.Succeeded)
		{
			_updateRequest = project.Data.Adapt<P004Request>();
		}
		_loaded = true;
	}

	public async Task SubmitAsync()
	{
		var updateResult = await _projectService.UpdateProjectAsync(_updateRequest);
		if (updateResult.Succeeded)
		{
			_navManager.NavigateTo($"/projects/{_updateRequest.Id.ToString()}");
		}
		else
		{
			updateResult.Messages.ForEach(x => _snackBar.Add(x, Severity.Error));
		}
	}
}