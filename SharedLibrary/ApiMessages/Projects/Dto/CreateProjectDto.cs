using FluentValidation;
using SharedLibrary.ApiMessages.Constants;

namespace SharedLibrary.ApiMessages.Projects.Dto;

public class CreateProjectDto
{
	public string Name { get; set; } = String.Empty;
	public string Description { get; set; } = String.Empty;
	public string SystemRequirements { get; set; } = String.Empty;
	public string ExeFileName { get; set; } = String.Empty;
	public List<TagDto> Tags { get; set; } = new();
}

public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
{
	public CreateProjectDtoValidator()
	{
		RuleFor(x => x.Name)
		   .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(ValidateErrorMessages.NotEmpty)
		   .Must(x => x.Length <= 50).WithMessage(ValidateErrorMessages.MustBeLessThan(50));
		RuleFor(x => x.Description)
			.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(ValidateErrorMessages.NotEmpty)
			.Must(x => x.Length <= 400).WithMessage(ValidateErrorMessages.MustBeLessThan(400));
		RuleFor(x => x.ExeFileName)
			.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(ValidateErrorMessages.NotEmpty)
			.Must(x => x.Length <= 50).WithMessage(ValidateErrorMessages.MustBeLessThan(50));
		RuleFor(x => x.SystemRequirements)
			.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(ValidateErrorMessages.NotEmpty)
			.Must(x => x.Length <= 400).WithMessage(ValidateErrorMessages.MustBeLessThan(400));
		RuleFor(x => x.Tags)
		   .Must(x => x.Count > 0 && x.Count <= 5).WithMessage("Минимум 1, максимум 5 тегов");

	}
}