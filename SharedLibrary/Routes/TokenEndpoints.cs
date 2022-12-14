using SharedLibrary.ApiMessages.Identity.ID001;
using SharedLibrary.ApiMessages.Identity.ID002;
using SharedLibrary.Wrapper;

namespace SharedLibrary.Routes;

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
    /// Request - <see cref="ID001Request"/>
    /// </para>
    /// <para>
    /// Response - <see cref="ID001Response"/>
    /// </para>
    /// </summary>
    public const string Base = "api/identity/token";

    /// <summary>
    /// POST - Refresh user access token
    /// <para>
    /// Request - <see cref="ID002Request"/>
    /// </para>
    /// <para>
    /// Response - <see cref="ID001Response"/>
    /// </para>
    /// </summary>
    /// <returns></returns>
    public static string RefreshToken() => $"{Base}/refresh";
}
