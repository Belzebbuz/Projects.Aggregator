using FluentValidation;
using MediatR;
using SharedLibrary.ApiMessages.Constants;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.P017;

/// <summary>
/// Create patch note
/// </summary>
public class P017Request : IRequest<IResult>
{
	public P017Request()
	{
	}
	public P017Request(Guid projectId, string text)
	{
		ProjectId = projectId;
		Text = text;
	}

	public Guid ProjectId { get; set; }
	public string Text { get; set; }
}

public class P017RequestValidator : AbstractValidator<P017Request>
{
	public P017RequestValidator()
	{
		RuleFor(x => x.Text)
			.Must(x => !string.IsNullOrEmpty(x)).WithMessage(ValidateErrorMessages.NotEmpty);
	}
}