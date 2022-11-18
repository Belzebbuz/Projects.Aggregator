using MediatR;
using SharedLibrary.ApiMessages.Projects.Dto;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.P003;

/// <summary>
/// Request for initial creating project
/// <para>
/// create tags if id is default
/// </para>
/// </summary>
public class P003Request : CreateProjectDto, IRequest<IResult>
{
    public P003Request()
    {
    }
    public P003Request(string name, string description, string systemRequirements,
        string exeFileName, List<TagDto> tags)
    {
        Name = name;
        Description = description;
        SystemRequirements = systemRequirements;
        ExeFileName = exeFileName;
        Tags = tags;
    }
}