using MediatR;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.M025;

/// <summary>
/// Create multiple tags 
/// </summary>
/// <param name="tagsNames"></param>
public record M025Request(ICollection<string> TagsNames) : IRequest<IResult<List<Guid>>>;
