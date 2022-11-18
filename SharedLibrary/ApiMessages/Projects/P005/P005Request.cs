using MediatR;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.P005;

/// <summary>
/// Add release file
/// </summary>
/// <param name="id"></param>
/// <param name="FileStream"></param>
public record P005Request(Guid ProjectId, string FileName, Stream FileStream) : IRequest<IResult>;
