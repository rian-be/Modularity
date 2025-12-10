using Signals.Core.Bus;
using Signals.Core.Events;
using Signals.Runtime.Loader;

namespace Signals.Extensions;

/// <summary>
/// Provides dynamic publishing helpers for <see cref="IEventBus"/> using runtime-loaded Signals.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Allows publishing events using string-based Signal and event identifiers.</item>
/// <item>Resolves event types at runtime via <see cref="SignalsLoader"/>.</item>
/// <item>Creates event instances using reflection and constructor arguments.</item>
/// <item>Invokes the standard <see cref="IEventBus.Publish(IEvent)"/> pipeline.</item>
/// <item>Intended for dynamic/plugin-based Signal execution scenarios.</item>
/// </list>
/// </remarks>
public static class EventBusExtensions
{
    /// <param name="bus">Event bus instance.</param>
    extension(IEventBus bus)
    {
        public async Task Publish(SignalsLoader loader,
            string signalsId,
            string eventName,
            params object[] ctorArgs)
        {
            var type = loader.GetPluginEventType(signalsId, eventName);
            if (type == null)
                throw new InvalidOperationException($"Event '{eventName}' not found in Signal '{signalsId}'.");

            var instance = (IEvent)Activator.CreateInstance(type, ctorArgs)!;
            await bus.Publish(instance);
        }

        /// <summary>
        /// Subscribes a handler to a dynamically-resolved plugin event type.
        /// </summary>
        /// <param name="loader">Signals loader to resolve event types.</param>
        /// <param name="signalsId">Plugin/Signal identifier.</param>
        /// <param name="eventName">Event type name inside the Signal.</param>
        /// <param name="handler">Handler to execute when the event is published.</param>
        public void Subscribe(SignalsLoader loader,
            string signalsId,
            string eventName,
            Func<IEvent, Task> handler)
        {
            var type = loader.GetPluginEventType(signalsId, eventName);
            if (type == null)
                throw new InvalidOperationException($"Event '{eventName}' not found in Signal '{signalsId}'.");

            bus.Subscribe(type, handler);
        }
        
        /// <summary>
        /// Unsubscribes a handler from a dynamically-resolved event type.
        /// </summary>
        /// <param name="loader">Signals loader to resolve event types.</param>
        /// <param name="signalsId">Plugin/Signal identifier.</param>
        /// <param name="eventName">Event type name inside the Signal.</param>
        /// <param name="handler">Handler to remove.</param>
        public void Unsubscribe(SignalsLoader loader,
            string signalsId,
            string eventName,
            Func<IEvent, Task> handler)
        {
            var type = loader.GetPluginEventType(signalsId, eventName);
            if (type == null)
                throw new InvalidOperationException($"Event '{eventName}' not found in Signal '{signalsId}'.");

            bus.Unsubscribe(type, handler);
        }

        /// <summary>
        /// Unsubscribes a strongly-typed handler using runtime type.
        /// </summary>
        /// <param name="eventType">Runtime event type.</param>
        /// <param name="handler">Handler to remove.</param>
        public void Unsubscribe(Type eventType,
            Func<IEvent, Task> handler)
        {
            if (!typeof(IEvent).IsAssignableFrom(eventType))
                throw new ArgumentException("Type must implement IEvent", nameof(eventType));

            var method = typeof(IEventBus)
                .GetMethod(nameof(IEventBus.Unsubscribe), [typeof(Type), typeof(Func<IEvent, Task>)])!;

            method.Invoke(bus, [eventType, handler]);
        }

        /// <summary>
        /// Convenience wrapper for strongly-typed unsubscribe.
        /// </summary>
        public void Unsubscribe<TEvent>(Func<TEvent, Task> handler)
            where TEvent : IEvent
        {
            bus.Unsubscribe(typeof(TEvent), evt => handler((TEvent)evt));
        }
    }
}