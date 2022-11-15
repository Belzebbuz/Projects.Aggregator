using App.Shared.ApiMessages.Projects.M015;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.ApiMessages.Projects.M009;
using SharedLibrary.ApiMessages.Projects.M011;
using SharedLibrary.ApiMessages.Projects.M012;
using SharedLibrary.ApiMessages.Projects.M020;
using SharedLibrary.ApiMessages.Projects.M021;
using SharedLibrary.ApiMessages.Projects.M019;
using SharedLibrary.ApiMessages.Projects.M024;
using SharedLibrary.ApiMessages.Projects.M025;
using SharedLibrary.Wrapper;
using System;

namespace SharedLibrary.Routes;

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
	/// <para>
	/// <strong>POST</strong> - Request <see cref="M024Request"/>
	/// </para>
	/// <para>
	///<strong>PUT</strong> - Request <see cref="M015Request"/>
	/// </para>
	/// <para>
	/// <strong>GET</strong> - Response <see cref="M021Response"/>
	/// </para>
	/// </summary>
	/// <returns></returns>
	public static string GetTagsRoute() => $"{Base}/tags";

    /// <summary>
    /// <strong>POST</strong> - Request <see cref="M025Request"/>
    /// </summary>
    /// <returns></returns>
	public static string GetMultipleTagsRoute() => $"{Base}/multiple_tags";

	/// <summary>
	///  <para>
	/// <strong>GET</strong> - Response <see cref="IList{T}"/> of <see cref="TagDto"/>
	/// </para>
	/// </summary>
	/// <returns></returns>
	public static string GetTagsFilterRoute(string text) => $"{Base}/tags/{text}";
	//
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

	/// <summary>
	/// <strong>POST</strong> - Request <see cref="M019Request"/>
	/// </summary>
	/// <returns></returns>
	public static string GetReleaseNoteRoute() => $"{Base}/releaseNotes";

    /// <summary>
    /// <strong>GET</strong> - Response <see cref="ICollection{T}"/> of <see cref="string"/>
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string GetProjectsNamesRoute(string text) => $"{Base}/names/{text}";
}
