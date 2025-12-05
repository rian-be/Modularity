using System.Collections.Concurrent;
using Signals.Core.Events;
using Signals.Core.Handlers;
using Signals.Delegates;
using Signals.Pipeline;

namespace Signals.Core.Bus;

/// <summary>
/// Responsible for publishing events to subscribed handlers via a middleware pipeline.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Maintains a collection of handlers per event type (<see cref="HandlerCollection"/>).</item>
/// <item>Executes middleware pipeline before invoking final handlers.</item>
/// <item>Supports batch publishing of events using a shared <see cref="EventContext"/>.</item>
/// <item>Handles "once-only" handlers and removes them after invocation.</item>
/// <item>Catches and logs exceptions in handlers to avoid breaking the pipeline.</item>
/// <item>Thread-safe via <see cref="ConcurrentDictionary{TKey,TValue}"/> and immutable snapshots.</item>
/// </list>
/// </remarks>
public sealed class Publisher : IPublisher
{
    private readonly ConcurrentDictionary<Type, HandlerCollection> _handlers;
    private readonly EventDelegate _pipeline;

    public Publisher(
        ConcurrentDictionary<Type, HandlerCollection> handlers,
        IEnumerable<IEventMiddleware> middlewares)
    {
        _handlers = handlers;
        _pipeline = middlewares.Reverse()
            .Aggregate(
                (EventDelegate)InternalPublish,
                (next, mw) => (evt, ctx) => mw.InvokeAsync(evt, ctx, next)
            );
    }
    
    /// <inheritdoc />
    public Task Publish(IEvent evt, EventContext? ctx = null)
        => _pipeline(evt, ctx ?? EventContext.Create());

    /// <inheritdoc />
    public Task PublishBatch(params IEvent[] events)
    {
        var ctx = EventContext.Create();
        var tasks = events.Select(e => Publish(e, ctx));
        return Task.WhenAll(tasks);
    }
    
    /// <summary>
    /// Internal method that executes all subscribed handlers for a given event.
    /// </summary>
    /// <param name="evt">Event to process.</param>
    /// <param name="ctx">Context providing correlation, tracing, and user information.</param>
    private async Task InternalPublish(IEvent evt, EventContext ctx)
    {
        if (!_handlers.TryGetValue(evt.GetType(), out var collection) || collection.IsEmpty)
            return;

        var snapshot = collection.GetSnapshot(evt);

        var tasks = snapshot.Select(async h =>
        {
            try
            {
                await h.Handler(evt);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Publisher] Exception in handler: {ex}");
            }
        });

        await Task.WhenAll(tasks);
        collection.RemoveOnceHandlers();
    }
}