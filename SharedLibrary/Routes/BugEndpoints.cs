using SharedLibrary.ApiMessages.BugReports.BG001;
using SharedLibrary.ApiMessages.BugReports.Dto;
using SharedLibrary.Wrapper;

namespace SharedLibrary.Routes;
/// <summary>
/// Contains projects API endpoints
/// <para>
/// All responses wrapped into <see cref="IResult"/>
/// </para>
/// </summary>
public static class BugEndpoints
{
	/// <summary>
	/// POST - Request <see cref="BG001Request"/>
	/// <para>
	/// GET - Response <see cref="PaginatedResult{T}"/> of <see cref="BugReportDto"/>
	/// </para>
	/// </summary>
	public const string Base = "bugReports";
}
