using Signals.Core.Context;
using Signals.Core.Events;
using Signals.Core.Subscriptions;

namespace Signals.Core.Bus;

/// <summary>
/// Central event bus that manages event subscriptions and publishing.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Implements <see cref="IEventBus"/> to provide a unified API for subscribing, unsubscribing, and publishing events.</item>
/// <item>Delegates subscription management to <see cref="ISubscriptionManager"/>.</item>
/// <item>Delegates publishing of events to <see cref="IPublisher"/>, including middleware execution.</item>
/// <item>Supports one-time handlers, handler filtering, and priority ordering.</item>
/// <item>Supports request/response messaging via <see cref="Send{TRequest,TResponse}"/>.</item>
/// <item>Creates <see cref="SignalContext"/> for each request/response invocation.</item>
/// <item>Uses <see cref="SignalCallGuard"/> to prevent recursive signal dispatch and excessive depth.</item>
/// <item>Middleware can be injected via DI to extend event processing (logging, filtering, tracing, metrics, etc.).</item>
/// </list>
/// </remarks>

public sealed class EventBus(
    ISubscriptionManager subscriptionManager,
    IPublisher publisher)
    : IEventBus
{
    public ISubscriptionManager SubscriptionManager { get; } = subscriptionManager;

    /// <inheritdoc />
    public SubscriptionToken Subscribe<TEvent>(
        Func<TEvent, Task> handler,
        int priority = 0,
        Func<TEvent, bool>? filter = null)
        where TEvent : IEvent
        => SubscriptionManager.Subscribe(handler, priority, filter);

    public SubscriptionToken SubscribeOnce<TEvent>(
        Func<TEvent, Task> handler,
        int priority = 0,
        Func<TEvent, bool>? filter = null)
        where TEvent : IEvent
        => SubscriptionManager.SubscribeOnce(handler, priority, filter);

    public bool Unsubscribe(SubscriptionToken token)
        => SubscriptionManager.Unsubscribe(token);

    public SubscriptionToken Subscribe(Type eventType, Func<IEvent, Task> handler)
    {
        if (!typeof(IEvent).IsAssignableFrom(eventType))
            throw new ArgumentException("Type must implement IEvent", nameof(eventType));

        var method = typeof(ISubscriptionManager)
            .GetMethod(nameof(ISubscriptionManager.Subscribe))!
            .MakeGenericMethod(eventType);

        return (SubscriptionToken)method.Invoke(
            SubscriptionManager,
            [handler, 0, null])!;
    }
    
    /// <inheritdoc />
    public Task Publish(IEvent evt)
        => publisher.Publish(evt);
    
    /// <inheritdoc />
    public Task PublishBatch(params IEvent[] events)
        => publisher.PublishBatch(events);

    /// <inheritdoc />
    public async Task<TResponse> Send<TRequest, TResponse>(TRequest request)
        where TRequest : IEvent
        where TResponse : IResponseEvent
    {
        if (SubscriptionManager is not IRequestHandlerRegistry registry)
            throw new InvalidOperationException(
                "EventBus's subscription manager must implement IRequestHandlerRegistry");

        var handler = registry.GetHandler<TRequest, TResponse>()
                      ?? throw new InvalidOperationException(
                          $"No handler registered for {typeof(TRequest).Name}");

        var ctx = new SignalContext(
            EventContext.Create(),
            this
        );

        var guard = new SignalCallGuard();
        using (guard.EnterScope<TRequest>())
        {
            return await handler.Handle(request, ctx);
        }
    }
}
