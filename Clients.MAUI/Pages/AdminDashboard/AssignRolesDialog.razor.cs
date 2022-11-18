using Clients.MAUI.Utilities;
using Microsoft.AspNetCore.Components;
using SharedLibrary.ApiMessages.Identity.Base;

namespace Clients.MAUI.Pages.AdminDashboard;

public partial class AssignRolesDialog
{
	[CascadingParameter] MudDialogInstance MudDialog { get; set; }
	[Parameter] public string UserId { get; set; }
	private List<UserRoleDto> _roles = new();
	private bool _loaded = false;
	protected override async Task OnInitializedAsync()
	{
		_loaded = false;
		var rolesResult = await _identityService.GetUserRoleAsync(UserId);
		if(rolesResult.Data != null)
			_roles = rolesResult.Data;
		if (!rolesResult.Succeeded)
			rolesResult.Messages.ForEach(x => _snackBar.Add(x, Severity.Error));
		_loaded = true;
	}

	private async Task AssignAsync()
	{
		var assignResult = await _identityService.AssignUserRolesAsync(new(UserId, _roles));
		if (assignResult.Succeeded)
			MudDialog.Close(DialogResult.Ok(true));
	}
}