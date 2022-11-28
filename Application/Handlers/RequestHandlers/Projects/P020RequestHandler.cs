using Application.Contracts.Repository;
using Application.Contracts.Services;
using Ardalis.Specification;
using Domain.Aggregators.ProjectAggregate;
using Mapster;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.ApiMessages.Projects.P020;
using SharedLibrary.Helpers;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class P020RequestHandler : IRequestHandler<P020Request, PaginatedResult<PatchNoteDto>>
{
	private readonly IReadRepository<Project> _readRepository;
	private readonly IPaginationService _paginationService;

	public P020RequestHandler(IReadRepository<Project> readRepository, IPaginationService paginationService)
	{
		_readRepository = readRepository;
		_paginationService = paginationService;
	}
	public async Task<PaginatedResult<PatchNoteDto>> Handle(P020Request request, CancellationToken cancellationToken)
	{
		var project = await _readRepository.SingleOrDefaultAsync(new GetProjectById(request.ProjectId));
		ThrowHelper.NotFoundEntity(project, request.ProjectId.ToString(), nameof(Project));
		var pagination = _paginationService.Calculate(project.PatchNotes.Count);
		var patchNotes = project.PatchNotes
			.OrderByDescending(x => x.LastModifiedOn)
			.Skip((pagination.Page - 1) * pagination.RecordsPerPage)
			.Take(pagination.RecordsPerPage)
			.Adapt<List<PatchNoteDto>>();
		return PaginatedResult<PatchNoteDto>.Success(patchNotes, project.PatchNotes.Count, pagination.Page, pagination.RecordsPerPage);
	}

	private class GetProjectById : Specification<Project>, ISingleResultSpecification<Project>
	{
		public GetProjectById(Guid projectId)
		{
			Query
				.AsNoTracking()
				.Include(x => x.PatchNotes)
				.Where(x => x.Id == projectId);
		}
	}
}

