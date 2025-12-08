using Signals.Core.Events;

namespace Signals.Core.Bus;

/// <summary>
/// Defines the central event bus responsible for publishing events and handling request/response messaging.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Supports asynchronous event publishing via <see cref="Publish"/> and <see cref="PublishBatch"/>.</item>
/// <item>Provides subscription management with priorities, filters, and one-time handlers.</item>
/// <item>Enables request/response communication using <see cref="Send{TRequest,TResponse}"/>.</item>
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
    /// <typeparam name="TEvent">Type of the event.</typeparam>
    /// <param name="handler">Asynchronous handler to execute when the event is published.</param>
    /// <param name="priority">Execution priority; higher values are executed first.</param>
    /// <param name="filter">Optional predicate to filter incoming events.</param>
    void Subscribe<TEvent>(
        Func<TEvent, Task> handler,
        int priority = 0,
        Func<TEvent, bool>? filter = null)
        where TEvent : IEvent;

    /// <summary>
    /// Subscribes a one-time handler to a specific event type.
    /// </summary>
    /// <typeparam name="TEvent">Type of the event.</typeparam>
    /// <param name="handler">Asynchronous handler to execute once.</param>
    /// <param name="priority">Execution priority; higher values are executed first.</param>
    /// <param name="filter">Optional predicate to filter incoming events.</param>
    void SubscribeOnce<TEvent>(
        Func<TEvent, Task> handler,
        int priority = 0,
        Func<TEvent, bool>? filter = null)
        where TEvent : IEvent;

    /// <summary>
    /// Unsubscribes a handler from a specific event type.
    /// </summary>
    /// <typeparam name="TEvent">Type of the event.</typeparam>
    /// <param name="handler">Handler instance to remove.</param>
    void Unsubscribe<TEvent>(Func<TEvent, Task> handler)
        where TEvent : IEvent;

    /// <summary>
    /// Subscribes a non-generic handler to a specific event type.
    /// </summary>
    /// <param name="eventType">Runtime type of the event.</param>
    /// <param name="handler">Asynchronous handler to execute.</param>
    void Subscribe(Type eventType, Func<IEvent, Task> handler);

    /// <summary>
    /// Unsubscribes a non-generic handler from a specific event type.
    /// </summary>
    /// <param name="eventType">Runtime type of the event.</param>
    /// <param name="handler">Handler instance to remove.</param>
    void Unsubscribe(Type eventType, Func<IEvent, Task> handler);

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
    Task<TResponse> Send<TRequest, TResponse>(TRequest request)
        where TRequest : IEvent
        where TResponse : IResponseEvent;
}
