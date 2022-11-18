using SharedLibrary.Authentication;

namespace Clients.MAUI.Pages.Settings;

public partial class Settings
{
	private string _userEmail;
	private string _serverVersion = string.Empty;

	protected override async Task OnInitializedAsync()
	{
		var state = await _authStateProvider.GetAuthenticationStateAsync();
		_userEmail = state.User.GetEmail();
		var resultServerVersion = await _appInfoService.GetServerVersionAsync();
		if (resultServerVersion.Data != null)
			_serverVersion = resultServerVersion.Data;
	}

	private async Task LogoutAsync()
	{
		await _authService.LogoutAsync();
		_navManager.NavigateTo("/");
	}
}