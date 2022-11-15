using App.Shared.ApiMessages.Projects.M010;
using App.Shared.ApiMessages.Projects.M011;
using App.Shared.ApiMessages.Projects.M012;
using App.Shared.ApiMessages.Projects.M009;
using App.Shared.ApiMessages.Projects.M015;
using App.Shared.ApiMessages.Projects.M020;
using App.Shared.ApiMessages.Projects.M021;
using App.Shared.ApiMessages.Projects.Dto;
using App.Shared.Wrapper;

namespace App.Shared.Routes;

/// <summary>
/// Contains projects API endpoints
/// <para>
/// All responses wrapped into <see cref="IResult"/>
/// </para>
/// </summary>
public class ProjectsEndpoints
{
	/// <summary>
	/// Base path to projects API
	/// <para>
	/// <list type="bullet">
	/// <item>
	/// <strong>GET</strong> - Get list of project short dto
	/// <para>
	///		Response - wrapped into <see cref="PaginatedResult{T}"/>  <see cref="ProjectShortDto"/>
	/// </para>
	/// </item>
	/// <item>
	/// <strong>POST</strong> - Initial create project
	/// <para>
	///		Request - <see cref="M011Request"/>
	/// </para>
	/// </item>
	/// <item>
	/// <strong>PUT</strong> - Update project info
	/// <para>
	///		Request - <see cref="M012Request"/>
	/// </para>
	/// </item>
	/// </list>
	/// </para>
	/// </summary>
	public const string Base = "api/projects";

	/// <summary>
	/// <strong>GET</strong> - Response <see cref="M009Request"/>
	/// <para>
	/// <strong>DELETE</strong>
	/// </para>
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public static string GetProjectRoute(Guid id) => $"{Base}/{id}";

	/// <summary>
	/// POST
	/// </summary>
	/// <param name="projectId"></param>
	/// <returns></returns>
	public static string GetReleasesRoute(Guid projectId) => $"{Base}/{projectId}/releases";

	/// <summary>
	/// GET (Download)
	/// DELETE
	/// </summary>
	/// <param name="projectId"></param>
	/// <param name="releaseId"></param>
	/// <returns></returns>
	public static string GetSingleReleaseRoute(Guid projectId, Guid releaseId) => $"{Base}/{projectId}/releases/{releaseId}";

	/// <summary>
	/// <strong>POST</strong> - Request <see cref="M015Request"/>
	///  <para>
	/// <strong>GET</strong> - Response <see cref="M021Response"/>
	/// </para>
	/// </summary>
	/// <returns></returns>
	public static string GetTagsRoute() => $"{Base}/tags";

	/// <summary>
	///  <para>
	/// <strong>DELETE</strong>
	/// </para>
	/// </summary>
	/// <param name="projectId"></param>
	/// <param name="tagId"></param>
	/// <returns></returns>
	public static string GetSingleProjectTagRoute(Guid projectId, Guid tagId) => $"{Base}/{projectId}/tags/{tagId}";

	/// <summary>
	/// <strong>POST</strong>
	/// <para>
	///		Request - <see cref="M020Request"/>
	/// </para>
	/// <para>
	///		Response - wrapped into <see cref="PaginatedResult{T}"/>  <see cref="ProjectShortDto"/>
	/// </para>
	/// </summary>
	/// <returns></returns>
	public static string GetProjectFilterRoute() => $"{Base}/filter";

	public static string GetReleaseNoteRoute() => $"{Base}/releaseNotes";

}
