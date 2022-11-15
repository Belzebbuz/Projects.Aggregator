using Application.Contracts.Repository;
using Domain.Aggregators.Project;
using SharedLibrary.ApiMessages.Projects.M024;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class M024RequestHandler : IRequestHandler<M024Request, IResult<Guid>>
{
	private readonly IRepository<Tag> _repository;

	public M024RequestHandler(IRepository<Tag> repository) => _repository = repository;
	public async Task<IResult<Guid>> Handle(M024Request request, CancellationToken cancellationToken)
	{
		var newTag = Tag.Create(request.Value);
		await _repository.AddAsync(newTag);
		return Result<Guid>.Success(newTag.Id);
	}
}
