using Application.Contracts.Repository;
using Ardalis.Specification;
using Domain.Aggregators.Project;
using SharedLibrary.ApiMessages.Projects.P007;
using SharedLibrary.Helpers;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class P007RequestHandler : IRequestHandler<P007Request, IResult>
{
    private readonly IRepository<Project> _repository;
    private readonly IRepository<Tag> _tagRepository;

    public P007RequestHandler(IRepository<Project> repository, IRepository<Tag> tagRepository)
    {
        _repository = repository;
        _tagRepository = tagRepository;
    }
    public async Task<IResult> Handle(P007Request request, CancellationToken cancellationToken)
    {
        var project = await _repository.SingleOrDefaultAsync(new GetProjectById(request.ProjectId));
        ThrowHelper.NotFoundEntity(project, request.ProjectId.ToString(), nameof(Project));

        var tag = await _tagRepository.SingleOrDefaultAsync(new GetTagById(request.TagId));
        ThrowHelper.NotFoundEntity(tag, request.TagId.ToString(), nameof(Tag));

        project.AddTag(tag);
        await _repository.SaveChangesAsync();
        return Result.Success();
    }

    private class GetProjectById : Specification<Project>, ISingleResultSpecification<Project>
    {
        internal GetProjectById(Guid projectId)
            => Query
            .AsSplitQuery()
            .Where(x => x.Id == projectId)
            .Include(x => x.Releases)
            .Include(x => x.Tags);
    }

    private class GetTagById : Specification<Tag>, ISingleResultSpecification<Tag>
    {
        public GetTagById(Guid tagId)
            => Query.Where(x => x.Id == tagId);
    }
}
