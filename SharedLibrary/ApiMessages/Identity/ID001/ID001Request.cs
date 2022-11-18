using FluentValidation;

namespace SharedLibrary.ApiMessages.Identity.ID001;

/// <summary>
/// Token request message
/// </summary>
/// <param name="Email"></param>
/// <param name="Password"></param>
public class ID001Request
{
    public ID001Request()
    {
    }
    public ID001Request(string email, string password)
    {
        Email = email;
        Password = password;
    }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class ID001RequestValidator : AbstractValidator<ID001Request>
{
    public ID001RequestValidator()
    {
        RuleFor(p => p.Email).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid Email Address.");

        RuleFor(p => p.Password).Cascade(CascadeMode.Stop)
            .NotEmpty();
    }
}

