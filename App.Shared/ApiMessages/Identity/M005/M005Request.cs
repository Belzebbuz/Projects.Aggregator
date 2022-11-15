using App.Shared.ApiMessages.Constants;
using FluentValidation;
using MediatR;

namespace App.Shared.ApiMessages.Identity.M005;
/// <summary>
/// User registraion request
/// </summary>
public class M005Request
{
	public M005Request()
	{
	}
	public M005Request(
		string firstName, 
		string lastName, 
		string email, 
		string userName, 
		string password, 
		string confirmPassword, 
		string phoneNumber)
	{
		FirstName = firstName;
		LastName = lastName;
		Email = email;
		UserName = userName;
		Password = password;
		ConfirmPassword = confirmPassword;
		PhoneNumber = phoneNumber;
	}

	public string FirstName { get; set; }
	public string? LastName { get; set; }
	public string Email { get; set; }
	public string? UserName { get; set; }
	public string Password { get; set; }
	public string ConfirmPassword { get; set; }
	public string? PhoneNumber { get; set; }
}

public class M005RequestValidator : AbstractValidator<M005Request>
{
	public M005RequestValidator()
	{
		RuleFor(message => message.FirstName)
		.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(ValidateErrorMessages.NotEmpty);
		RuleFor(message => message.Email)
				.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(ValidateErrorMessages.NotEmpty)
				.EmailAddress().WithMessage("Поле должно соответсвовать типу email");
		RuleFor(request => request.Password)
					.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(ValidateErrorMessages.NotEmpty)
					.MinimumLength(8).WithMessage("Пароль должен быть не менее 8 символов")
					.Matches(@"[A-Z]").WithMessage("Пароль должен содержать минимум один символ верхнего регистра")
					.Matches(@"[a-z]").WithMessage("Пароль должен содержать минимум один символ нижнего регистра")
					.Matches(@"[0-9]").WithMessage("Пароль должен содержать минимум одну цифру");
		RuleFor(request => request.ConfirmPassword)
					.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(ValidateErrorMessages.NotEmpty)
					.Equal(request => request.Password).WithMessage("Пароли должны совпадать");
	}
}