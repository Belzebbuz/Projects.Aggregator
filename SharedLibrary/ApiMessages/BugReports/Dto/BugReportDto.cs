using SharedLibrary.ApiMessages.Projects.Dto;

namespace SharedLibrary.ApiMessages.BugReports.Dto;

public class BugReportDto : AuditDto
{
	public string Text { get; set; }
}
