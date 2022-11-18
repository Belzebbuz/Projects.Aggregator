using FluentValidation;
using MediatR;
using SharedLibrary.ApiMessages.Constants;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Identity.ID009;

/// <summary>
/// Change password request
/// </summary>
public class ID009Request : IRequest<IResult>
{
	public string OldPassword { get; set; }
	public string NewPassword { get; set; }
	public string ConfirmNewPassword { get; set; }
}

public class ID009RequestValidator : AbstractValidator<ID009Request>
{
	public ID009RequestValidator()
	{
		RuleFor(request => request.OldPassword)
				.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(ValidateErrorMessages.NotEmpty);
		RuleFor(request => request.NewPassword)
				.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(ValidateErrorMessages.NotEmpty)
				.MinimumLength(8).WithMessage("Пароль должен быть не менее 8 символов")
				.Matches(@"[A-Z]").WithMessage("Пароль должен содержать минимум один символ верхнего регистра")
				.Matches(@"[a-z]").WithMessage("Пароль должен содержать минимум один символ нижнего регистра")
				.Matches(@"[0-9]").WithMessage("Пароль должен содержать минимум одну цифру");
		RuleFor(request => request.ConfirmNewPassword)
				.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(ValidateErrorMessages.NotEmpty)
				.Equal(request => request.NewPassword).WithMessage("Пароли должны совпадать");
	}
}
