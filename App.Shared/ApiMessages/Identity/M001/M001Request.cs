using FluentValidation;

namespace App.Shared.ApiMessages.Identity.M001;

/// <summary>
/// Token request message
/// </summary>
/// <param name="Email"></param>
/// <param name="Password"></param>
public class M001Request
{
	public M001Request()
	{
	}
	public M001Request(string email, string password)
	{
		Email = email;
		Password = password;
	}
	public string Email { get; set; }
	public string Password { get; set; }
}

public class M001RequestValidator : AbstractValidator<M001Request>
{
	public M001RequestValidator()
	{
		RuleFor(p => p.Email).Cascade(CascadeMode.Stop)
			.NotEmpty()
			.EmailAddress()
			.WithMessage("Invalid Email Address.");

		RuleFor(p => p.Password).Cascade(CascadeMode.Stop)
			.NotEmpty();
	}
}

