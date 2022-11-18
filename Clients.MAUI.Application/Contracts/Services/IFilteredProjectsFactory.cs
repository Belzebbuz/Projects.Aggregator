using Clients.MAUI.Application.Contracts.Services.Common;
using Clients.MAUI.Application.Contracts.ViewModels;

namespace Clients.MAUI.Application.Contracts.Services;

public interface IFilteredProjectsFactory : ISingletonService
{
	public IFilteredContainer Create();
}
