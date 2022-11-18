using MediatR;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.P004;

/// <summary>
/// Update project base info
/// </summary>
public class P004Request : CreateProjectDto, IRequest<IResult>
{
    public Guid Id { get; set; }
}