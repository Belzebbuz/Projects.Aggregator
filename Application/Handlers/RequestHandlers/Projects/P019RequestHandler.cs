using Application.Contracts.Repository;
using Ardalis.Specification;
using Domain.Aggregators.Project;
using SharedLibrary.ApiMessages.Projects.P019;
using SharedLibrary.Helpers;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class P019RequestHandler : IRequestHandler<P019Request, IResult>
{
	private readonly IRepository<Project> _repository;

	public P019RequestHandler(IRepository<Project> repository) => _repository = repository;
	public async Task<IResult> Handle(P019Request request, CancellationToken cancellationToken)
	{
		var project = await _repository.SingleOrDefaultAsync(new GetProjectById(request.ProjectId, request.PatchNoteId));
		ThrowHelper.NotFoundEntity(project, request.ProjectId.ToString(), nameof(Project));
		project.RemovePatchNote(request.PatchNoteId);
		await _repository.SaveChangesAsync();
		return Result.Success();
	}

	private class GetProjectById : Specification<Project>, ISingleResultSpecification<Project>
	{
		public GetProjectById(Guid projectId, Guid patchNoteId)
		{
			Query
				.Include(x => x.PatchNotes.Where(x => x.Id == patchNoteId))
				.Where(x => x.Id == projectId);
		}
	}
}

