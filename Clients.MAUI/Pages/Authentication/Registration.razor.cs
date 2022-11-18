using Clients.MAUI.Utilities;
using SharedLibrary.ApiMessages.Identity.ID005;
namespace Clients.MAUI.Pages.Authentication;

public partial class Registration
{
    FluentValidationValidator? _fluentValidationValidator;
    private bool _validated => _fluentValidationValidator.Validate(options => options.IncludeAllRuleSets());
    private ID005Request _registerRequest = new();
    public async Task SubmitAsync()
    {
        var result = await _identityService.SelfRegisterAsync(_registerRequest);
        _snackBar.HandleResult(result, () => _navManager.NavigateTo("login"));
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