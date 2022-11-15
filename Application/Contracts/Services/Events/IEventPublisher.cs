using Application.Contracts.DI;
using SharedLibrary;

namespace Application.Contracts.Services.Events;

public interface IEventPublisher : ITransientService
{
    Task PublishAsync(IEvent @event);
}