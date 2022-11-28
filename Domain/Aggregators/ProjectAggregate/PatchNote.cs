using Domain.Base;
using SharedLibrary.Helpers;

namespace Domain.Aggregators.ProjectAggregate;

public class PatchNote : AuditableEntity
{
	public string Text { get; private set; }

	internal PatchNote(string text)
	{
		ThrowHelper.NotNull(text, nameof(text));
		Text = text;
	}

	internal void Update(string text)
	{
		ThrowHelper.NotNull(text, nameof(text));
		Text = text;
	}
}