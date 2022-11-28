using Domain.Base;
using SharedLibrary.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Domain.Aggregators.BugAggregate;

public class BugReport : AuditableEntity, IAggregateRoot
{
	[MaxLength(1000)]
	public string Text { get; private set; }
	public string? ImageUrl { get; private set; }
	private BugReport()
	{
	}

	public static BugReport Create(string text, string? imageUrl = null)
	{
		ThrowHelper.NotNull(text, nameof(text));
		return new()
		{
			Text = text,
			ImageUrl = imageUrl,
		};
	}
}
