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
    #region Event Handlers

    /// <inheritdoc />
    public void Subscribe<TEvent>(Func<TEvent, Task> handler, int priority = 0, Func<TEvent, bool>? filter = null) where TEvent : IEvent
        => AddHandler(handler, once: false, priority, filter);

    /// <inheritdoc />
    public void SubscribeOnce<TEvent>(Func<TEvent, Task> handler, int priority = 0, Func<TEvent, bool>? filter = null) where TEvent : IEvent
        => AddHandler(handler, once: true, priority, filter);

    /// <inheritdoc />
    public void Unsubscribe<TEvent>(Func<TEvent, Task> handler) where TEvent : IEvent
    {
        if (handlers.TryGetValue(typeof(TEvent), out var collection))
            collection.Unsubscribe(e => handler((TEvent)e));
    }

    #endregion
    
    private void AddHandler<TEvent>(Func<TEvent, Task> handler, bool once, int priority, Func<TEvent, bool>? filter) where TEvent : IEvent
    {
        var wrapper = new HandlerWrapper(
            Handler: e => handler((TEvent)e),
            Priority: priority,
            Filter: filter is not null ? new Func<IEvent, bool>(e => filter((TEvent)e)) : null,
            Once: once
        );
        
        handlers.GetOrAdd(typeof(TEvent), _ => new HandlerCollection()).Add(wrapper);
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
