using System.Collections.ObjectModel;

namespace App.Shared.Authentication;

public class SHRoles
{
	public const string Admin = nameof(Admin);
	public const string Basic = nameof(Basic);
	public const string Dev = nameof(Dev);
	public static IReadOnlyList<string> DefaultRoles { get; } = new ReadOnlyCollection<string>(new[]
	{
		Admin,
		Dev,
		Basic
	});
}
