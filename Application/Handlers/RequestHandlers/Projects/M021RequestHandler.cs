using Application.Contracts.Repository;
using Domain.Aggregators.Project;
using Mapster;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.ApiMessages.Projects.M021;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class M021RequestHandler : IRequestHandler<M021Request, IResult<M021Response>>
{
    private readonly IReadRepository<Tag> _repository;

    public M021RequestHandler(IReadRepository<Tag> repository) => _repository = repository;
    public async Task<IResult<M021Response>> Handle(M021Request request, CancellationToken cancellationToken)
    {
        var tags = await _repository.ListAsync();
        var tagsDto = tags.Adapt<ICollection<TagDto>>();
        return Result<M021Response>.Success(data: new(tagsDto));
    }
}
