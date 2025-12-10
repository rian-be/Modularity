using Signals.Core.Events;
using Signals.Core.Subscriptions;

namespace Signals.Core.Bus;

/// <summary>
/// Defines the central event bus responsible for publishing events and handling request/response messaging.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Supports asynchronous event publishing via <see cref="Publish"/> and <see cref="PublishBatch"/>.</item>
/// <item>Provides subscription management with priorities, filters, and one-time handlers.</item>
/// <item>Enables request/response communication using <see cref="Send{TRequest,TResponse}"/>.</item>
/// <item>Returns <see cref="SubscriptionToken"/> for handler registration, enabling unsubscribing.</item>
/// <item>Acts as the core messaging backbone for the Signals runtime.</item>
/// </list>
/// </remarks>
public interface IEventBus
{
    /// <summary>
    /// Publishes a single event to all registered subscribers.
    /// </summary>
    /// <param name="evt">Event instance to publish.</param>
    Task Publish(IEvent evt);

    /// <summary>
    /// Publishes multiple events as a batch.
    /// </summary>
    /// <param name="events">Collection of events to publish.</param>
    Task PublishBatch(params IEvent[] events);

    /// <summary>
    /// Subscribes a handler to a specific event type.
    /// </summary>
    /// <typeparam name="TEvent">Type of the event to subscribe to.</typeparam>
    /// <param name="handler">Asynchronous handler function.</param>
    /// <param name="priority">Optional priority for handler execution order.</param>
    /// <param name="filter">Optional filter to conditionally invoke the handler.</param>
    /// <returns>A <see cref="SubscriptionToken"/> representing this subscription.</returns>
    SubscriptionToken Subscribe<TEvent>(
        Func<TEvent, Task> handler,
        int priority = 0,
        Func<TEvent, bool>? filter = null)
        where TEvent : IEvent;

    
    SubscriptionToken SubscribeOnce<TEvent>(
        Func<TEvent, Task> handler,
        int priority = 0,
        Func<TEvent, bool>? filter = null)
        where TEvent : IEvent;
    
    SubscriptionToken Subscribe(Type eventType, Func<IEvent, Task> handler);
    
    bool Unsubscribe(SubscriptionToken token);
    
    /// <summary>
    /// Sends a request event and awaits a typed response.
    /// </summary>
    /// <typeparam name="TRequest">Type of the request event.</typeparam>
    /// <typeparam name="TResponse">Type of the response event.</typeparam>
    /// <param name="request">Request event to send.</param>
    /// <returns>Asynchronous response produced by the registered request handler.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no handler is registered for the given request type.
    /// </exception>
    Task<TResponse> Send<TRequest, TResponse>(
        TRequest request)
        where TRequest : IEvent
        where TResponse : IResponseEvent;

}
