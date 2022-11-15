using FluentValidation;
using MediatR;
using SharedLibrary.ApiMessages.Constants;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.M012;

/// <summary>
/// Update project base info
/// </summary>
public class M012Request : CreateProjectDto, IRequest<IResult>
{
    public Guid Id { get; set; }
}

public class M012RequestValidator : AbstractValidator<M012Request>
{
    public M012RequestValidator()
    {
        RuleFor(x => x.Name)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(ValidateErrorMessages.NotEmpty);
        RuleFor(x => x.Description)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(ValidateErrorMessages.NotEmpty);
        RuleFor(x => x.ExeFileName)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(ValidateErrorMessages.NotEmpty);
        RuleFor(x => x.SystemRequirements)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(ValidateErrorMessages.NotEmpty);
    }
}