using System.Collections.Concurrent;
using Signals.Core.Events;
using Signals.Core.Handlers;

namespace Signals.Core.Subscriptions;

/// <summary>
/// Manages subscriptions of event handlers for different event types.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Stores and organizes <see cref="HandlerWrapper"/> instances per <see cref="IEvent"/> type.</item>
/// <item>Supports regular and one-time handlers via <see cref="Subscribe"/> and <see cref="SubscribeOnce"/>.</item>
/// <item>Allows optional filtering of events and priority-based ordering of handlers.</item>
/// <item>Supports unsubscribing handlers via <see cref="Unsubscribe"/>.</item>
/// <item>Thread-safe via <see cref="ConcurrentDictionary{TKey,TValue}"/>.</item>
/// </list>
/// </remarks>
public sealed class SubscriptionManager(ConcurrentDictionary<Type, HandlerCollection> handlers) : ISubscriptionManager
{
    /// <inheritdoc />
    public void Subscribe<TEvent>(Func<TEvent, Task> handler, int priority = 0, Func<TEvent, bool>? filter = null) where TEvent : IEvent
    {
        var wrapper = new HandlerWrapper(
            Handler: e => handler((TEvent)e),
            Priority: priority,
            Filter: filter is not null
                ? (Func<IEvent, bool>)(e => filter((TEvent)e))
                : null,
            Once: false
        );
        
        handlers.GetOrAdd(typeof(TEvent), _ => new HandlerCollection()).Add(wrapper);
    }

    /// <inheritdoc />
    public void SubscribeOnce<TEvent>(Func<TEvent, Task> handler, int priority = 0, Func<TEvent, bool>? filter = null) where TEvent : IEvent
    {
        var wrapper = new HandlerWrapper(
            Handler: e => handler((TEvent)e),
            Priority: priority,
            Filter: filter is not null
                ? (Func<IEvent, bool>)(e => filter((TEvent)e))
                : null,
            Once: true
        );

        handlers.GetOrAdd(typeof(TEvent), _ => new HandlerCollection()).Add(wrapper);
    }
    
    /// <inheritdoc />
    public void Unsubscribe<TEvent>(Func<TEvent, Task> handler) where TEvent : IEvent
    {
        if (handlers.TryGetValue(typeof(TEvent), out var collection))
            collection.Unsubscribe(e => handler((TEvent)e));
    }
}