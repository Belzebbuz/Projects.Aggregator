using Application.Contracts.Repository;
using Ardalis.Specification;
using Domain.Aggregators.ProjectAggregate;
using SharedLibrary.ApiMessages.Projects.P018;
using SharedLibrary.Helpers;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class P018RequestHandler : IRequestHandler<P018Request, IResult>
{
	private readonly IRepository<Project> _repository;

	public P018RequestHandler(IRepository<Project> repository) => _repository = repository;
	public async Task<IResult> Handle(P018Request request, CancellationToken cancellationToken)
	{
		var project = await _repository.SingleOrDefaultAsync(new GetProjectById(request.ProjectId, request.PatchNoteId));
		ThrowHelper.NotFoundEntity(project, request.ProjectId.ToString(), nameof(Project));
		project.UpdatePatchNote(request.ProjectId, request.Text);
		await _repository.SaveChangesAsync();
		return Result.Success();
	}

	private class GetProjectById : Specification<Project>, ISingleResultSpecification<Project>
	{
		public GetProjectById(Guid projectId, Guid patchNoteId)
		{
			Query
				.Include(x => x.PatchNotes.SingleOrDefault(x => x.Id == patchNoteId))
				.Where(x => x.Id == projectId);
		}
	}
}

