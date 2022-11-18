using Application.Contracts.Repository;
using Ardalis.Specification;
using Domain.Aggregators.Project;
using Mapster;
using SharedLibrary.ApiMessages.Projects.P003;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class P003RequestHandler : IRequestHandler<P003Request, IResult>
{
    private readonly IRepository<Project> _repository;
    private readonly IRepository<Tag> _tagRepository;

    public P003RequestHandler(IRepository<Project> repository, IRepository<Tag> tagRepository)
    {
        _repository = repository;
        _tagRepository = tagRepository;
    }
    public async Task<IResult> Handle(P003Request request, CancellationToken cancellationToken)
    {
        var exists = await _repository.AnyAsync(new GetProjectByName(request.Name));
        if (exists)
            return Result.Fail($"Project with Name: \"{request.Name}\" already exists!");

        var tagsWithId = request.Tags.Where(x => x.Id != default).Select(x => x.Id).ToList();
        var namesOfTagsWithOutId = request.Tags.Where(x => x.Id == default).Select(x => x.Value).ToList();
		var tags = await _tagRepository.ListAsync(new GetTagsByIds(tagsWithId));
        tags.AddRange(namesOfTagsWithOutId.Select(x => Tag.Create(x)));
        var project = Project.Create(request.Name, request.Description, request.ExeFileName, request.SystemRequirements, tags);
        await _repository.AddAsync(project);
        return Result.Success(project.Id.ToString());
    }

    private class GetProjectByName : Specification<Project>
    {
        public GetProjectByName(string name)
         => Query
            .Where(x => x.Name == name);
    }

    private class GetTagsByIds : Specification<Tag>
    {
        public GetTagsByIds(ICollection<Guid> tags)
            => Query
            .Where(x => tags.Contains(x.Id));
    }
}
