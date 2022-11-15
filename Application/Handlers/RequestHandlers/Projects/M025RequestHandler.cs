using Application.Contracts.Repository;
using Domain.Aggregators.Project;
using SharedLibrary.ApiMessages.Projects.M025;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class M025RequestHandler : IRequestHandler<M025Request, IResult<List<Guid>>>
{
	private readonly IRepository<Tag> _repository;

	public M025RequestHandler(IRepository<Tag> repository) => _repository = repository;
	public async Task<IResult<List<Guid>>> Handle(M025Request request, CancellationToken cancellationToken)
	{
		var newTags = request.TagsNames.Select(x => Tag.Create(x));
		await _repository.AddRangeAsync(newTags);
		return Result<List<Guid>>.Success(newTags.Select(x =>x.Id).ToList());
	}
}
