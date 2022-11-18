using SharedLibrary.ApiMessages.Identity.Base;
using SharedLibrary.ApiMessages.Identity.ID004;
using SharedLibrary.ApiMessages.Identity.ID005;
using SharedLibrary.ApiMessages.Identity.ID006;
using SharedLibrary.ApiMessages.Identity.ID007;
using SharedLibrary.ApiMessages.Identity.ID008;
using SharedLibrary.ApiMessages.Identity.ID009;
using SharedLibrary.ApiMessages.Identity.ID010;
using SharedLibrary.Wrapper;

namespace SharedLibrary.Routes;
/// <summary>
/// Contains users API endpoints
/// <para>
/// All responses wrapped into <see cref="IResult"/>
/// </para>
/// </summary>
public class UsersEndpoints
{
    /// <summary>
    /// Root path for users API
    /// <para>
    /// <list type="bullet">
    /// <item>
    ///		<strong>GET</strong> - Get list of all users
    ///		<para>
    ///			Response - wrapped into <see cref="PaginatedResult{T}"/> <see cref="UserDto"/>
    ///		</para>
    /// </item>
    /// <item>
    ///		<strong>POST</strong> - Create new user
    ///		<para>
    ///			Request - <see cref="ID005Request"/>
    ///		</para>
    /// </item>
    /// </list>
    /// </para>
    /// </summary>
    public const string Base = "api/identity/users";

    private const string _roles = "roles";
    private const string _selfRegister = "self-register";
    private const string _toggleStatus = "toggle-status";
    private const string _changePassword = "change-password";
	/// <summary>
	/// POST - Assign new roles for user
	/// <para>
	/// Request - <see cref="ID008Request"/>
	/// </para>
	/// </summary>
	/// <returns></returns>
	public static string GetRolesRoute() => $"{Base}/{_roles}";

    /// <summary>
    /// GET - Get single user info
    /// <para>
    ///	Response - <see cref="ID007Response"/>
    /// </para>
    /// </summary>
    /// <param name="id">User Id</param>
    /// <returns></returns>
    public static string GetUserRoute(string id) => $"{Base}/{id}";

	/// <summary>
	/// POST - Get single user info
	/// <para>
	///	Request - <see cref="ID010Request"/>
	/// </para>
	/// <para>
	/// Response - <see cref="PaginatedResult{T}"/> <see cref="UserDto"/>
	/// </para>
	/// </summary>
	/// <returns></returns>
	public static string GetUserByFilterRoute() => $"{Base}/search";

	/// <summary>
	/// GET - Get user roles
	/// <para>
	/// Response - <see cref="ID004Response"/>
	/// </para>
	/// </summary>
	/// <param name="id">User Id</param>
	/// <returns></returns>
	public static string GetUserRolesRoute(string id) => $"{Base}/{id}/{_roles}";

    /// <summary>
    /// POST - Toogle user IsActive status
    /// <para>
    /// Request - <see cref="ID006Request"/>
    /// </para>
    /// </summary>
    /// <returns></returns>
    public static string GetToggleUserStatusRoute() => $"{Base}/{_toggleStatus}";

    /// <summary>
    /// POST - Allow anonymous create user
    /// <para>
    /// Request - <see cref="ID005Request"/>
    /// </para>
    /// </summary>
    /// <returns></returns>
    public static string GetSelfRegisterRoute() => $"{Base}/{_selfRegister}";

    /// <summary>
    /// POST - change current user password
    /// </summary>
    /// <para>
    /// Request - <see cref="ID009Request"/>
    /// </para>
    /// <returns></returns>
    public static string GetChangePasswordRoute() => $"{Base}/{_changePassword}";
}
