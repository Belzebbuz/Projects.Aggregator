using Clients.MAUI.Application.Contracts.Services;
using Clients.MAUI.Application.Contracts.ViewModels;

namespace Clients.MAUI.Infrastructure.Projects;

public class FilteredProjectsFactory : IFilteredProjectsFactory
{
    private readonly IProjectService _projectService;

    public FilteredProjectsFactory(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public IFilteredContainer Create()
    {
        return new FilteredProjects(_projectService);
    }
}
