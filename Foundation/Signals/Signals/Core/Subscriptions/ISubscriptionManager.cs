using System.Collections.Concurrent;
using Signals.Core.Bus;
using Signals.Core.Events;
using Signals.Core.Handlers;

namespace Signals.Core.Subscriptions;

/// <summary>
/// Provides methods to manage subscriptions of event handlers.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Supports subscribing regular and one-time handlers for <see cref="IEvent"/> types.</item>
/// <item>Allows optional filtering and priority ordering of handlers.</item>
/// <item>Uses token-based unsubscription for deterministic and safe removal.</item>
/// <item>Supports fast lookup of the first handler for a given event type.</item>
/// <item>Designed to be used internally by <see cref="IEventBus"/> implementations.</item>
/// <item>Thread-safe via <see cref="ConcurrentDictionary{TKey,TValue}"/>.</item>
/// </list>
/// </remarks>
public interface ISubscriptionManager : IRequestHandlerRegistry
{
    /// <summary>
    /// Gets the first registered handler for a given event type based on priority ordering.
    /// </summary>
    /// <param name="eventType">Runtime type of the event.</param>
    /// <returns>The highest-priority <see cref="HandlerWrapper"/>, or <c>null</c> if none exists.</returns>
    HandlerWrapper? GetFirstHandler(Type eventType);
    
    /// <summary>
    /// Subscribes a handler for a specific event type.
    /// </summary>
    /// <typeparam name="TEvent">Type of event. Must implement <see cref="IEvent"/>.</typeparam>
    /// <param name="handler">Asynchronous handler function.</param>
    /// <param name="priority">Handler priority; higher values are invoked first.</param>
    /// <param name="filter">Optional predicate to filter events.</param>
    /// <returns>
    /// A <see cref="SubscriptionToken"/> that uniquely identifies this subscription
    /// and can be used to unsubscribe.
    /// </returns>
    SubscriptionToken Subscribe<TEvent>(
        Func<TEvent, Task> handler,
        int priority = 0,
        Func<TEvent, bool>? filter = null)
        where TEvent : IEvent;
    
    /// <summary>
    /// Subscribes a one-time handler for a specific event type.
    /// The handler is automatically removed after the first invocation.
    /// </summary>
    /// <typeparam name="TEvent">Type of event. Must implement <see cref="IEvent"/>.</typeparam>
    /// <param name="handler">Asynchronous handler function.</param>
    /// <param name="priority">Handler priority; higher values are invoked first.</param>
    /// <param name="filter">Optional predicate to filter events.</param>
    /// <returns>
    /// A <see cref="SubscriptionToken"/> that uniquely identifies this subscription
    /// until it is executed and removed.
    /// </returns>
    SubscriptionToken SubscribeOnce<TEvent>(
        Func<TEvent, Task> handler,
        int priority = 0,
        Func<TEvent, bool>? filter = null)
        where TEvent : IEvent;

    /// <summary>
    /// Unsubscribes a previously registered handler using its subscription token.
    /// </summary>
    /// <param name="token">Token returned during subscription.</param>
    /// <returns>
    /// <c>true</c> if the handler was successfully removed;
    /// <c>false</c> if the token was not found.
    /// </returns>
    bool Unsubscribe(SubscriptionToken token);
}