using Clients.MAUI.Pages.SharedForms.Dialogs;
using Clients.MAUI.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using SharedLibrary.ApiMessages.Identity.ID001;
using System.Security.Claims;

namespace Clients.MAUI.Pages.Authentication;

public partial class Login
{
    private FluentValidationValidator? _fluentValidationValidator;
    private bool Validated => _fluentValidationValidator!.Validate(options => { options.IncludeAllRuleSets(); });
    private ID001Request _tokenRequest = new();

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

    public async Task SetBaseAddressAsync()
    {
        var baseAddress = await _appInfoService.GetServerUrlAsync();
        var parameters = new DialogParameters();
        parameters.Add(nameof(SelectServerAddress.ServerAddress), baseAddress);
		var dialog = _dialogService.Show<SelectServerAddress>("Установить адрес сервера", parameters, new DialogOptions() { FullWidth = true, CloseButton = true });
		var dialogResult = await dialog.Result;
        if (!dialogResult.Cancelled)
        {
			await _appInfoService.SetServerUrlAsync((string)dialogResult.Data);
            _snackBar.Add("Нужно перезайти в приложение!", Severity.Warning);
		}
	}
}