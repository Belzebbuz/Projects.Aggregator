using SharedLibrary.Authentication;

namespace Clients.MAUI.Shared;

public partial class MainLayout
{
    private MudTheme _currentTheme = MudUISettings.DefaultTheme;
	protected override async Task OnInitializedAsync()
	{
		var state = await _authStateProvider.GetAuthenticationStateAsync();
		var user = state.User;
		if (user == null)
			return;

		if (user.Identity?.IsAuthenticated == true)
		{
			var currentUserResult = await _identityService.GetUserAsync(user.GetUserId());
			if (!currentUserResult.Succeeded || currentUserResult.Data == null)
			{
				_snackBar.Add("You are logged out because the user with your Token has been deleted.", Severity.Error);
				await _authService.LogoutAsync();
			}
		}

		
	}

}