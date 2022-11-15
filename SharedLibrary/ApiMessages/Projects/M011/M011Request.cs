using FluentValidation;
using MediatR;
using SharedLibrary.ApiMessages.Constants;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.M011;

/// <summary>
/// Request for initial creating project
/// <para>
/// create tags if id is default
/// </para>
/// </summary>
public class M011Request : CreateProjectDto, IRequest<IResult>
{
    public M011Request()
    {
    }
    public M011Request(string name, string description, string systemRequirements,
        string exeFileName, List<TagDto> tags)
    {
        Name = name;
        Description = description;
        SystemRequirements = systemRequirements;
        ExeFileName = exeFileName;
        Tags = tags;
    }
}

public class M011RequestValidator : AbstractValidator<M011Request>
{
    public M011RequestValidator()
    {
        RuleFor(x => x.Name)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(ValidateErrorMessages.NotEmpty);
        RuleFor(x => x.Description)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(ValidateErrorMessages.NotEmpty);
        RuleFor(x => x.ExeFileName)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(ValidateErrorMessages.NotEmpty);
        RuleFor(x => x.SystemRequirements)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(ValidateErrorMessages.NotEmpty);
		RuleFor(x => x.Tags)
		   .Must(x => x.Count > 0).WithMessage("Должен быть добавлен хотя бы один тег");

	}
}