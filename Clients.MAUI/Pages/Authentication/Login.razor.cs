using Clients.MAUI.Utilities;
using Microsoft.AspNetCore.Components
/* Unmerged change from project 'Clients.MAUI (net6.0-maccatalyst)'
Before:
using System.Security.Claims;
using Clients.MAUI.Utilities;
After:
using Microsoft.AspNetCore.Components.Authorization;
*/

/* Unmerged change from project 'Clients.MAUI (net6.0-ios)'
Before:
using System.Security.Claims;
using Clients.MAUI.Utilities;
After:
using Microsoft.AspNetCore.Components.Authorization;
*/

/* Unmerged change from project 'Clients.MAUI (net6.0-windows10.0.19041.0)'
Before:
using System.Security.Claims;
using Clients.MAUI.Utilities;
After:
using Microsoft.AspNetCore.Components.Authorization;
*/
.Authorization;
using SharedLibrary.ApiMessages.Identity.M001;
using System.Security.Claims;

namespace Clients.MAUI.Pages.Authentication;

public partial class Login
{
    private FluentValidationValidator? _fluentValidationValidator;
    private bool Validated => _fluentValidationValidator!.Validate(options => { options.IncludeAllRuleSets(); });
    private M001Request _tokenRequest = new();

    protected override async Task OnInitializedAsync()
    {
        var state = await _authStateProvider.GetAuthenticationStateAsync();
        if (state != new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())))
        {
            _navManager.NavigateTo("/");
        }
    }
    public async Task SubmitAsync()
    {
        var result = await _authService.LoginAsync(_tokenRequest);
        _snackBar.HandleResult(result, () =>
        {
            _navManager.NavigateTo("/", true);
        });
    }

    private bool _passwordVisibility;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    private void TogglePasswordVisibility()
    {
        if (_passwordVisibility)
        {
            _passwordVisibility = false;
            _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
            _passwordInput = InputType.Password;
        }
        else
        {
            _passwordVisibility = true;
            _passwordInputIcon = Icons.Material.Filled.Visibility;
            _passwordInput = InputType.Text;
        }
    }
}