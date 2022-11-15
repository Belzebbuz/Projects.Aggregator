using App.Shared.ApiMessages.Identity.M001;
using App.Shared.ApiMessages.Identity.M002;
using App.Shared.Wrapper;

namespace App.Shared.Routes;

/// <summary>
/// Contains token API endpoints
/// <para>
/// All responses wrapped into <see cref="IResult"/>
/// </para>
/// </summary>
public class TokenEndpoints
{
	/// <summary>
	/// POST - Get user access token
	/// <para>
	/// Request - <see cref="M001Request"/>
	/// </para>
	/// <para>
	/// Response - <see cref="M001Response"/>
	/// </para>
	/// </summary>
	public const string Base = "api/identity/token";

	/// <summary>
	/// POST - Refresh user access token
	/// <para>
	/// Request - <see cref="M002Request"/>
	/// </para>
	/// <para>
	/// Response - <see cref="M001Response"/>
	/// </para>
	/// </summary>
	/// <returns></returns>
	public static string RefreshToken() => $"{Base}/refresh";
}
