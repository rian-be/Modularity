using Signals.Core.Events;

namespace Signals.Core.Handlers;

public interface IEventHandler<in TEvent> where TEvent : IEvent
{
    Task HandleAsync(TEvent evt);
}