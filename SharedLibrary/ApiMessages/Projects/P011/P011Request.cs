using MediatR;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.P011;

/// <summary>
/// Add or update release note request
/// </summary>
/// <param name="ProjectId"></param>
/// <param name="ReleaseId"></param>
/// <param name="ReleaseNote"></param>
public record P011Request(Guid ProjectId, Guid ReleaseId, string? ReleaseNote) : IRequest<IResult>;
