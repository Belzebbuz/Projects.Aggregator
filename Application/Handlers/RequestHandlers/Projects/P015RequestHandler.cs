using Application.Contracts.Repository;
using Ardalis.Specification;
using Domain.Aggregators.Project;
using Mapster;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.ApiMessages.Projects.P015;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class P015RequestHandler : IRequestHandler<P015Request, IResult<List<TagDto>>>
{
	private readonly IReadRepository<Tag> _repository;

	public P015RequestHandler(IReadRepository<Tag> repository) => _repository = repository;
	public async Task<IResult<List<TagDto>>> Handle(P015Request request, CancellationToken cancellationToken)
	{
		var tags = await _repository.ListAsync(new GetTagsByName(request.text));
		return Result<List<TagDto>>.Success(data: tags);
	}

	public class GetTagsByName : Specification<Tag, TagDto>
	{
		public GetTagsByName(string? text)
		{
			Query.OrderBy(x => x.Value);
			if (!string.IsNullOrEmpty(text))
			{
				Query.Search(x => x.Value, $"%{text}%");
			}
			Query.Adapt<TagDto>();
		}
	}
}
