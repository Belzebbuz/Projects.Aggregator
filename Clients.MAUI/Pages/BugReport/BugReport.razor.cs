using Blazored.FluentValidation;
using Clients.MAUI.Utilities;
using SharedLibrary.ApiMessages.BugReports.BG001;
using SharedLibrary.ApiMessages.BugReports.Dto;

namespace Clients.MAUI.Pages.BugReport;

public partial class BugReport
{
	private FluentValidationValidator? _fluentValidationValidator;
	private bool _validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
	private BG001Request _bugRequest = new();
	private List<BugReportDto> _reports = new();

    protected override async Task OnInitializedAsync()
    {
        var reportsResult = await _bugReportService.GetBugReportsAsync(1, 10);
        _reports = _snackBar.HandleResult(reportsResult)?.Data;
        
    }
    private async Task SubmitAsync()
	{
		var result = await _bugReportService.SendBugReportAsync(_bugRequest);
		_snackBar.HandleResult(result, () => _navManager.NavigateTo("/"));
	}
}