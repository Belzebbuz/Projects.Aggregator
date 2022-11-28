using Application.Contracts.Repository;
using Domain.Aggregators.ProjectAggregate;
using Mapster;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.ApiMessages.Projects.P013;
using SharedLibrary.Wrapper;

namespace Application.Handlers.RequestHandlers.Projects;

public class P013RequestHandler : IRequestHandler<P013Request, IResult<P013Response>>
{
    private readonly IReadRepository<Tag> _repository;

    public P013RequestHandler(IReadRepository<Tag> repository) => _repository = repository;
    public async Task<IResult<P013Response>> Handle(P013Request request, CancellationToken cancellationToken)
    {
        var tags = await _repository.ListAsync();
        var tagsDto = tags.Adapt<ICollection<TagDto>>();
        return Result<P013Response>.Success(data: new(tagsDto));
    }
}
