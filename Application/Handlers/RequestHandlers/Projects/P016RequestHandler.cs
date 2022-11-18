using Application.Contracts.Repository;
using Domain.Aggregators.Project;
using SharedLibrary.ApiMessages.Projects.P016;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class P016RequestHandler : IRequestHandler<P016Request, IResult<Guid>>
{
	private readonly IRepository<Tag> _repository;

	public P016RequestHandler(IRepository<Tag> repository) => _repository = repository;
	public async Task<IResult<Guid>> Handle(P016Request request, CancellationToken cancellationToken)
	{
		var newTag = Tag.Create(request.Value);
		await _repository.AddAsync(newTag);
		return Result<Guid>.Success(newTag.Id);
	}
}
