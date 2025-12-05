using System.Collections.Concurrent;
using Signals.Interfaces;

namespace Signals;

/// <summary>
/// Central event bus that manages event subscriptions and publishing.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Implements <see cref="IEventBus"/> to provide a unified API for subscribing, unsubscribing, and publishing events.</item>
/// <item>Delegates subscription management to <see cref="ISubscriptionManager"/>.</item>
/// <item>Delegates publishing of events to <see cref="IPublisher"/>, including middleware execution.</item>
/// <item>Supports one-time handlers, handler filtering, and priority ordering.</item>
/// <item>Thread-safe via <see cref="ConcurrentDictionary{TKey,TValue}"/> and internal handler collections.</item>
/// <item>Middleware can be injected via DI to extend event processing (logging, filtering, tracing, metrics, etc.).</item>
/// </list>
/// </remarks>
public sealed class EventBus(ISubscriptionManager subscriptionManager, IPublisher publisher)
    : IEventBus
{
    /// <inheritdoc />
    public void Subscribe<TEvent>(
        Func<TEvent, Task> handler,
        int priority = 0,
        Func<TEvent, bool>? filter = null)
        where TEvent : IEvent
        => subscriptionManager.Subscribe(handler, priority, filter);
    
    /// <inheritdoc />
    public void Subscribe<TEvent>(Func<TEvent, EventContext, Task> handler) where TEvent : IEvent
    {
        Subscribe((Func<TEvent, Task>)Wrapper);
        return;

        Task Wrapper(TEvent evt)
        {
            var ctx = EventContext.Create();
            return handler(evt, ctx);
        }
    }
    
    public void SubscribeOnce<TEvent>(
        Func<TEvent, Task> handler,
        int priority = 0,
        Func<TEvent, bool>? filter = null)
        where TEvent : IEvent
        => subscriptionManager.SubscribeOnce(handler, priority, filter);

    /// <inheritdoc />
    public void Unsubscribe<TEvent>(Func<TEvent, Task> handler)
        where TEvent : IEvent
        => subscriptionManager.Unsubscribe(handler);

    public void Unsubscribe<TEvent>(Func<TEvent, EventContext, Task> handler) where TEvent : IEvent
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void Subscribe(Type eventType, Func<IEvent, Task> handler)
    {
        if (!typeof(IEvent).IsAssignableFrom(eventType))
            throw new ArgumentException("Type must implement IEvent", nameof(eventType));

        var method = typeof(ISubscriptionManager)
            .GetMethod(nameof(ISubscriptionManager.Subscribe))!
            .MakeGenericMethod(eventType);
        method.Invoke(subscriptionManager, [handler, 0, null]);
    }

    /// <inheritdoc />
    public void Unsubscribe(Type eventType, Func<IEvent, Task> handler)
    {
        if (!typeof(IEvent).IsAssignableFrom(eventType))
            throw new ArgumentException("Type must implement IEvent", nameof(eventType));

        var method = typeof(ISubscriptionManager)
            .GetMethod(nameof(ISubscriptionManager.Unsubscribe))!
            .MakeGenericMethod(eventType);
        method.Invoke(subscriptionManager, [handler]);
    }

    /// <inheritdoc />
    public Task Publish(IEvent evt)
        => publisher.Publish(evt);

    /// <inheritdoc />
    public Task PublishBatch(params IEvent[] events)
        => publisher.PublishBatch(events);
}