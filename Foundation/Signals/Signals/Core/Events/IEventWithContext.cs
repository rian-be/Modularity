using Signals.Core.Context;

namespace Signals.Core.Events;

/// <summary>
/// Represents an event that carries a mutable <see cref="EventContext"/>.
/// </summary>
/// <remarks>
/// Useful for scenarios where the event pipeline needs to pass context information
/// (e.g., correlation IDs, trace metadata) along with the event.
/// </remarks>
public interface IEventWithContext : IEvent
{
    /// <summary>
    /// Sets the event context for this event.
    /// </summary>
    /// <param name="ctx">The <see cref="EventContext"/> to associate with the event.</param>
    void SetContext(EventContext ctx);

    /// <summary>
    /// Gets the event context associated with this event, if any.
    /// </summary>
    EventContext? Context { get; }
}