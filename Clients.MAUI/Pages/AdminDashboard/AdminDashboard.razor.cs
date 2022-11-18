using Clients.MAUI.Pages.SharedForms.Dialogs;
using Clients.MAUI.Utilities;
using SharedLibrary.ApiMessages.Identity.Base;

namespace Clients.MAUI.Pages.AdminDashboard;

public partial class AdminDashboard
{
	private List<UserDto> _users = new();
	private bool _isLoaded = false;
	private int _itemsPerPage = 5;
	private int _selectedPage = 1;
	private int _totalPages;	
	private string _searchString = string.Empty;
	protected override async Task OnInitializedAsync()
	{
		_isLoaded = false;
		await LoadUsersAsync();
		_isLoaded = true;
	}

	private async Task LoadUsersAsync(int selectedPage = 1)
	{
		_selectedPage = selectedPage;
		var usersResult = await _identityService.SearchUsersAsync(_searchString , selectedPage, _itemsPerPage);
		if(usersResult.Data != null)
			_users = usersResult.Data;
		if (!usersResult.Succeeded)
		{
			usersResult.Messages.ForEach(message => _snackBar.Add(message, Severity.Error));
			_navManager.NavigateTo("/");
		}
	}

	private async Task SearchStringChangedAsync(string text)
	{
		_searchString = text;
		await LoadUsersAsync();
	}

	private async Task AsignRolesAsync(string userId)
	{
		var parameters = new DialogParameters();
		parameters.Add(nameof(AssignRolesDialog.UserId), userId);
		var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
		var dialog = _dialogService.Show<AssignRolesDialog>("Фильтр по тегам", parameters, options);
		var dialogResult = await dialog.Result;
	}

	private async Task DeleteAsync(UserDto user)
	{
		var parameters = new DialogParameters
			{
				{nameof(DeleteConfirmDialog.ContentText), $"Пользователь {user.Email}." }
			};
		var dialog = _dialogService.Show<DeleteConfirmDialog>("Удаление пользователя", parameters, new DialogOptions() { FullWidth = true, CloseButton = true });
		var dialogResult = await dialog.Result;
		if (!dialogResult.Cancelled)
		{
			var deleteResult = await _identityService.DeleteUserAsync(user.Id);
			_snackBar.HandleResult(deleteResult);
			await LoadUsersAsync();
		}
	}

	private async Task ToogleUserIsActiveAsync(bool isActive, UserDto userDto)
	{
		var result = await _identityService.ToggleUserStatusAsync(userDto.Id, isActive);
		_snackBar.HandleResult(result);
		await LoadUsersAsync();
	}
}