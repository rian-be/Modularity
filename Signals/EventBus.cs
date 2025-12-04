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
    /*
    public EventBus(ISubscriptionManager subscriptionManager, IPublisher publisher)
    {
        _subscriptionManager = subscriptionManager;
        _publisher = publisher;
    }
    */

    /// <inheritdoc />
    public void Subscribe<TEvent>(
        Func<TEvent, Task> handler,
        int priority = 0,
        Func<TEvent, bool>? filter = null)
        where TEvent : IEvent
        => subscriptionManager.Subscribe(handler, priority, filter);

    /// <inheritdoc />
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

    /// <inheritdoc />
    public Task Publish(IEvent evt)
        => publisher.Publish(evt);

    /// <inheritdoc />
    public Task PublishBatch(params IEvent[] events)
        => publisher.PublishBatch(events);
}