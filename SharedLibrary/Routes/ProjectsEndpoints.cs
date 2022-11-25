using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.ApiMessages.Projects.P001;
using SharedLibrary.ApiMessages.Projects.P003;
using SharedLibrary.ApiMessages.Projects.P004;
using SharedLibrary.ApiMessages.Projects.P012;
using SharedLibrary.ApiMessages.Projects.P013;
using SharedLibrary.ApiMessages.Projects.P011;
using SharedLibrary.ApiMessages.Projects.P016;
using SharedLibrary.Wrapper;
using SharedLibrary.ApiMessages.Projects.P017;
using SharedLibrary.ApiMessages.Projects.P018;
using SharedLibrary.ApiMessages.Projects.P007;

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
    ///		Request - <see cref="P003Request"/>
    /// </para>
    /// </item>
    /// <item>
    /// <strong>PUT</strong> - Update project info
    /// <para>
    ///		Request - <see cref="P004Request"/>
    /// </para>
    /// </item>
    /// </list>
    /// </para>
    /// </summary>
    public const string Base = "api/projects";

    /// <summary>
    /// <strong>GET</strong> - Response <see cref="P001Request"/>
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
    public static string GetReleasesRoute() => $"{Base}/releases";

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
	/// <strong>POST</strong> - Request <see cref="P016Request"/>
	/// </para>
	/// <para>
	///<strong>PUT</strong> - Request <see cref="P007Request"/>
	/// </para>
	/// <para>
	/// <strong>GET</strong> - Response <see cref="P013Response"/>
	/// </para>
	/// </summary>
	/// <returns></returns>
	public static string GetTagsRoute() => $"{Base}/tags";


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
    ///		Request - <see cref="P012Request"/>
    /// </para>
    /// <para>
    ///		Response - wrapped into <see cref="PaginatedResult{T}"/>  <see cref="ProjectShortDto"/>
    /// </para>
    /// </summary>
    /// <returns></returns>
    public static string GetProjectFilterRoute() => $"{Base}/filter";

	/// <summary>
	/// <strong>POST</strong> - Request <see cref="P011Request"/>
	/// </summary>
	/// <returns></returns>
	public static string GetReleaseNoteRoute() => $"{Base}/releaseNotes";


	/// <summary>
	/// POST - создать Request - <see cref="P017Request"/>
	/// <para>
	/// PUT - обновить Request - <see cref="P018Request"/>
	/// </para>
	/// DELETE - удалить
	/// GET - получить все
	/// </summary>
	/// <returns></returns>
	public static string GetPatchNoteRoute(Guid projectId) => $"{Base}/{projectId}/patchNotes";

    public static string GetSinglePatchNoteRoute(Guid projectId, Guid patchNoteId) => $"{Base}/{projectId}/patchNotes/{patchNoteId}";
}
