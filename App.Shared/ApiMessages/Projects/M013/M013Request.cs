using App.Shared.Wrapper;
using MediatR;

namespace App.Shared.ApiMessages.Projects.M013;

/// <summary>
/// Add release file
/// </summary>
/// <param name="id"></param>
/// <param name="FileStream"></param>
public record M013Request(Guid ProjectId, string FileName, Stream FileStream) : IRequest<IResult>;
