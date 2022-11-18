using MediatR;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.Wrapper;
using System.Diagnostics;

namespace SharedLibrary.ApiMessages.Projects.P012;

/// <summary>
/// Filter project request
/// </summary>
public class P012Request : IRequest<PaginatedResult<ProjectShortDto>>
{
    public P012Request()
    {
    }
    public P012Request(string? filterName, List<Guid>? tagIds = null)
    {
        FilterName = filterName;
        TagIds = tagIds ?? new List<Guid>();
    }

    public string? FilterName { get; set; }
    public List<Guid>? TagIds { get; set; } = new();
}

//public class M020Validator : AbstractValidator<P012Request>
//{
//	public M020Validator()
//	{
//		RuleFor(x => x.FilterName)
//			.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(ValidateErrorMessages.NotEmpty);
//	}
//}
