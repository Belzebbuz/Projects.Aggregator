using Clients.MAUI.Utilities;
using SharedLibrary.ApiMessages.Identity.ID009;

namespace Clients.MAUI.Pages.Settings;

public partial class ChangePassword
{
	private FluentValidationValidator? _fluentValidationValidator;
	private bool Validated => _fluentValidationValidator!.Validate(options => { options.IncludeAllRuleSets(); });
	private readonly ID009Request _changePasswordRequest = new();

	public async Task ChangePasswordAsync()
	{
		var result = await _identityService.ChangePasswordAsync(_changePasswordRequest);
		_snackBar.HandleResult(result, () =>
		{
			_snackBar.Add("Пароль изменен", Severity.Success);
			_changePasswordRequest.OldPassword = String.Empty;	
			_changePasswordRequest.NewPassword = String.Empty;	
			_changePasswordRequest.ConfirmNewPassword = String.Empty;	
		});
			
	}

	private bool _currentPasswordVisibility;
	private InputType _currentPasswordInput = InputType.Password;
	private string _currentPasswordInputIcon = Icons.Material.Filled.VisibilityOff;

	private bool _newPasswordVisibility;
	private InputType _newPasswordInput = InputType.Password;
	private string _newPasswordInputIcon = Icons.Material.Filled.VisibilityOff;

	private void TogglePasswordVisibility(bool newPassword)
	{
		if (newPassword)
		{
			if (_newPasswordVisibility)
			{
				_newPasswordVisibility = false;
				_newPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
				_newPasswordInput = InputType.Password;
			}
			else
			{
				_newPasswordVisibility = true;
				_newPasswordInputIcon = Icons.Material.Filled.Visibility;
				_newPasswordInput = InputType.Text;
			}
		}
		else
		{
			if (_currentPasswordVisibility)
			{
				_currentPasswordVisibility = false;
				_currentPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
				_currentPasswordInput = InputType.Password;
			}
			else
			{
				_currentPasswordVisibility = true;
				_currentPasswordInputIcon = Icons.Material.Filled.Visibility;
				_currentPasswordInput = InputType.Text;
			}
		}
	}
}