using App.Shared.ApiMessages.Constants;
using App.Shared.ApiMessages.Projects.Dto;
using App.Shared.ApiMessages.Projects.M011;
using App.Shared.Wrapper;
using FluentValidation;
using MediatR;

namespace App.Shared.ApiMessages.Projects.M012;

/// <summary>
/// Update project base info
/// </summary>
public class M012Request : IRequest<IResult>
{
	public M012Request()
	{
	}
	public M012Request(Guid projectId, string name, string description,
		string systemRequirements, string exeFileName, ICollection<Guid> tags)
	{
		ProjectId = projectId;
		Name = name;
		Description = description;
		SystemRequirements = systemRequirements;
		ExeFileName = exeFileName;
		TagsIds = tags;
	}

	public Guid ProjectId { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public string SystemRequirements { get; set; }
	public string ExeFileName { get; set; }
	public ICollection<Guid> TagsIds { get; set;} 
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