using Application.Contracts.Repository;
using Ardalis.Specification;
using Domain.Aggregators.Project;
using SharedLibrary.ApiMessages.Projects.P004;
using SharedLibrary.Helpers;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class P004RequestHandler : IRequestHandler<P004Request, IResult>
{
    private readonly IRepository<Project> _repository;
    private readonly IRepository<Tag> _tagRepository;

    public P004RequestHandler(IRepository<Project> repository, IRepository<Tag> tagRepository)
    {
        _repository = repository;
        _tagRepository = tagRepository;
    }
    public async Task<IResult> Handle(P004Request request, CancellationToken cancellationToken)
    {
        var project = await _repository.SingleOrDefaultAsync(new GetProjectById(request.Id));

        ThrowHelper.NotFoundEntity(project, request.Id.ToString(), nameof(Project));

		var tagsWithId = request.Tags.Where(x => x.Id != default).Select(x => x.Id).ToList();
		var namesOfTagsWithOutId = request.Tags.Where(x => x.Id == default).Select(x => x.Value).ToList();
		var tags = await _tagRepository.ListAsync(new GetTagsByIds(tagsWithId));
		tags.AddRange(namesOfTagsWithOutId.Select(x => Tag.Create(x)));

		project.Update(request.Name, request.Description, request.ExeFileName, request.SystemRequirements);
        project.UpdateTags(tags);
        await _repository.SaveChangesAsync();
        return Result.Success();
    }

    private class GetProjectById : Specification<Project>, ISingleResultSpecification<Project>
    {
        public GetProjectById(Guid Id)
            => Query
            .Include(x => x.Tags)
            .Where(x => x.Id == Id);
    }

    private class GetTagsByIds : Specification<Tag>
    {
        public GetTagsByIds(ICollection<Guid> tagsIds)
            => Query
            .Where(x => tagsIds.Contains(x.Id));
    }
}
