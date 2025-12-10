using System.Collections.Concurrent;
using Signals.Core.Bus;
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
public sealed class SubscriptionManager(
    ConcurrentDictionary<Type, HandlerCollection> handlers,
    ConcurrentDictionary<Type, RequestHandlerCollection> requestHandlers
    ) : ISubscriptionManager
{
    private long _id;
    
    #region Event Handlers

    /// <inheritdoc />
    public SubscriptionToken Subscribe<TEvent>(
        Func<TEvent, Task> handler,
        int priority = 0,
        Func<TEvent, bool>? filter = null)
        where TEvent : IEvent
        => AddHandler(handler, once: false, priority, filter);
  
    /// <inheritdoc />
    public SubscriptionToken SubscribeOnce<TEvent>(
        Func<TEvent, Task> handler,
        int priority = 0,
        Func<TEvent, bool>? filter = null)
        where TEvent : IEvent
        => AddHandler(handler, once: true, priority, filter);
    
    /// <inheritdoc />
    public bool Unsubscribe(SubscriptionToken token)
    {
        if (!handlers.TryGetValue(token.EventType, out var collection))
            return false;

        return collection.RemoveById(token.Id);
    }
    
    #endregion
    
    private SubscriptionToken AddHandler<TEvent>(
        Func<TEvent, Task> handler,
        bool once,
        int priority,
        Func<TEvent, bool>? filter)
        where TEvent : IEvent
    {
        var id = Interlocked.Increment(ref _id);

        var wrapper = new HandlerWrapper(
            id,
            e => handler((TEvent)e),
            priority,
            filter is null ? null : e => filter((TEvent)e),
            once
        );

        var collection = handlers.GetOrAdd(
            typeof(TEvent),
            _ => new HandlerCollection()
        );

        collection.Add(wrapper);

        return new SubscriptionToken(typeof(TEvent), id);
    }

    public HandlerWrapper? GetFirstHandler(Type eventType)
    {
        if (!handlers.TryGetValue(eventType, out var collection))
            return null;

        return collection.GetFirst();
    }
    #region Request Handlers

    public IRequestHandler<TRequest, TResponse>? GetHandler<TRequest, TResponse>()
        where TRequest : IEvent
        where TResponse : IResponseEvent
    {
        if (requestHandlers.TryGetValue(typeof(TRequest), out var collection))
            return collection.GetHandler<TRequest, TResponse>();
        return null;
    }

    public void RegisterHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler, IEventBus bus)
        where TRequest : IEvent
        where TResponse : IResponseEvent
    {
        var collection = requestHandlers.GetOrAdd(typeof(TRequest), _ => new RequestHandlerCollection());
        collection.SetHandler(handler);
    }

    #endregion
}
