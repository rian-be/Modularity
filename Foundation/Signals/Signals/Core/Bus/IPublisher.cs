using Signals.Core.Events;

namespace Signals.Core.Bus;

/// <summary>
/// Provides methods to publish events to subscribed handlers via an optional middleware pipeline.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Supports publishing single events or batches of events.</item>
/// <item>Handlers are executed according to priority, filtering, and once-only semantics.</item>
/// <item>Optionally integrates middleware to intercept, modify, or log events before they reach handlers.</item>
/// <item>Responsible for maintaining event context (<see cref="EventContext"/>) per event or per batch.</item>
/// </list>
/// </remarks>
public interface IPublisher
{
    /// <summary>
    /// Publishes a single event through the pipeline to all subscribed handlers.
    /// </summary>
    /// <param name="evt">The event to publish.</param>
    /// <param name="ctx">Optional <see cref="EventContext"/>; if null, a new context is created.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Publish(IEvent evt, EventContext? ctx = null);

    /// <summary>
    /// Publishes multiple events in a batch using a shared <see cref="EventContext"/>.
    /// </summary>
    /// <param name="events">Array of events to publish.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation for all events.</returns>
    Task PublishBatch(params IEvent[] events);
}