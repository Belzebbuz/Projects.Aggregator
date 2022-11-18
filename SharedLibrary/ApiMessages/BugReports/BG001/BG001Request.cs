using FluentValidation;
using MediatR;
using SharedLibrary.ApiMessages.Constants;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.BugReports.BG001;

public class BG001Request : IRequest<IResult>
{
    public string Text { get; set; } = string.Empty;
}

public class BG001RequestValidator : AbstractValidator<BG001Request>
{
    public BG001RequestValidator()
    {
        RuleFor(x => x.Text)
            .Must(x => !string.IsNullOrEmpty(x)).WithMessage(ValidateErrorMessages.NotEmpty)
            .Must(x => x.Length <= 1000).WithMessage(ValidateErrorMessages.MustBeLessThan(1000));
    }
}