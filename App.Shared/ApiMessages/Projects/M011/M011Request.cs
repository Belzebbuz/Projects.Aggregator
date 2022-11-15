using App.Shared.ApiMessages.Constants;
using App.Shared.ApiMessages.Projects.Dto;
using App.Shared.Wrapper;
using FluentValidation;
using MediatR;

namespace App.Shared.ApiMessages.Projects.M011;

/// <summary>
/// Request for initial creating project
/// </summary>
public class M011Request : IRequest<IResult>
{
	public M011Request()
	{
	}
	public M011Request(string name, string description, string systemRequirements, 
		string exeFileName, ICollection<Guid> tagsIds)
	{
		Name = name;
		Description = description;
		SystemRequirements = systemRequirements;
		ExeFileName = exeFileName;
		TagsIds = tagsIds;
	}

	public string Name { get; set; }
	public string Description { get; set; }
	public string SystemRequirements { get; set; }
	public string ExeFileName { get; set; }
	public ICollection<Guid> TagsIds { get; set; }
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
	}
}