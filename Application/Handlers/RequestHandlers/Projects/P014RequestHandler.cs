using Application.Contracts.Repository;
using Ardalis.Specification;
using Domain.Aggregators.Project;
using SharedLibrary.ApiMessages.Projects.P014;
using SharedLibrary.Helpers;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class P014RequestHandler : IRequestHandler<P014Request, IResult<List<string>>>
{
	private readonly IReadRepository<Project> _repository;

	public P014RequestHandler(IReadRepository<Project> repository) => _repository = repository;
	public async Task<IResult<List<string>>> Handle(P014Request request, CancellationToken cancellationToken)
	{
		var names = await _repository.ListAsync(new GetProjectsNamesByFilter(request.text));
		return Result<List<string>>.Success(names.Select(x => x.Name).ToList());
	}

	private class GetProjectsNamesByFilter : Specification<Project>
	{
		public GetProjectsNamesByFilter(string text)
		{
			Query
				.OrderBy(x => x.Name)
				.Take(10);
			if (!string.IsNullOrEmpty(text))
			{
				Query.Search(x => x.Name, $"%{text}%");
			}
		}
	}
}
