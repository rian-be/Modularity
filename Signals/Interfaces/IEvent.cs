namespace Signals.Interfaces;

/// <summary>
/// Represents a generic event in the event bus system.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Marker interface used to identify event types that can be published on <see cref="IEventBus"/>.</item>
/// <item>Concrete events should implement this interface.</item>
/// <item>Does not define any members; behavior is determined by consumers and producers.</item>
/// </list>
/// </remarks>
public interface IEvent
{
    
}