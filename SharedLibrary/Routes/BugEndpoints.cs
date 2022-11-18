using SharedLibrary.ApiMessages.BugReports.BG001;
using SharedLibrary.ApiMessages.BugReports.Dto;
using SharedLibrary.Wrapper;

namespace SharedLibrary.Routes;

public class BugEndpoints
{
	/// <summary>
	/// POST - Request <see cref="BG001Request"/>
	/// <para>
	/// GET - Response <see cref="PaginatedResult{T}"/> of <see cref="BugReportDto"/>
	/// </para>
	/// </summary>
	public const string Base = "bugReports";
}
