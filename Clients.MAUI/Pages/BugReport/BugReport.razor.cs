using Blazored.FluentValidation;
using Clients.MAUI.Utilities;
using SharedLibrary.ApiMessages.BugReports.BG001;

namespace Clients.MAUI.Pages.BugReport;

public partial class BugReport
{
	private FluentValidationValidator? _fluentValidationValidator;
	private bool _validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
	private BG001Request _bugRequest = new();
	
	private async Task SubmitAsync()
	{
		var result = await _bugReportService.SendBugReportAsync(_bugRequest);
		_snackBar.HandleResult(result, () => _navManager.NavigateTo("/"));
	}
}