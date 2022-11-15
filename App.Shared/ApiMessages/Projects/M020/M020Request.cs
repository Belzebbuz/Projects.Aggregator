using App.Shared.ApiMessages.Constants;
using App.Shared.ApiMessages.Projects.Dto;
using App.Shared.Wrapper;
using FluentValidation;
using MediatR;

namespace App.Shared.ApiMessages.Projects.M020;

/// <summary>
/// Filter project request
/// </summary>
public class M020Request  : IRequest<PaginatedResult<ProjectShortDto>>
{
	public M020Request()
	{
	}
	public M020Request(string filterName, ICollection<Guid> tagIds = null)
	{
		FilterName = filterName;
		TagIds = tagIds ?? new List<Guid>();
	}
	
	public string FilterName { get; set; }
	public ICollection<Guid>? TagIds { get; set; }
}

//public class M020Validator : AbstractValidator<M020Request>
//{
//	public M020Validator()
//	{
//		RuleFor(x => x.FilterName)
//			.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(ValidateErrorMessages.NotEmpty);
//	}
//}
