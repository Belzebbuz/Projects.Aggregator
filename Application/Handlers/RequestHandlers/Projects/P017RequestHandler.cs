using Application.Contracts.Repository;
using Ardalis.Specification;
using Domain.Aggregators.ProjectAggregate;
using SharedLibrary.ApiMessages.Projects.P017;
using SharedLibrary.Helpers;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class P017RequestHandler : IRequestHandler<P017Request, IResult>
{
	private readonly IRepository<Project> _repository;

	public P017RequestHandler(IRepository<Project> repository) => _repository = repository;
	public async Task<IResult> Handle(P017Request request, CancellationToken cancellationToken)
	{
		var project = await _repository.SingleOrDefaultAsync(new GetProjectById(request.ProjectId));
		ThrowHelper.NotFoundEntity(project, request.ProjectId.ToString(), nameof(Project));
		project.AddPatchNote(request.Text);
		await _repository.SaveChangesAsync();
		return Result.Success();
	}

	private class GetProjectById : Specification<Project>, ISingleResultSpecification<Project>
	{
		public GetProjectById(Guid projectId)
		{
			Query
				.Include(x => x.PatchNotes)
				.Where(x => x.Id == projectId);
		}
	}
}
