using Signals.Attributes;

namespace Signals.Interfaces;

/// <summary>
/// Represents a reactive component that consumes and produces events.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Subscribes to events on an <see cref="IEventBus"/> via <see cref="Subscribe"/>.</item>
/// <item>Exposes metadata through <see cref="Metadata"/> to describe the Eventlet (name, author, version, description).</item>
/// <item>Declares which event types it consumes (<see cref="Consumes"/>) and produces (<see cref="Produces"/>).</item>
/// <item>Used by the Eventlet loader to automatically register event handlers and build dependency graphs.</item>
/// <item>Implementations are responsible for wiring their own event handlers and reacting to events.</item>
/// </list>
/// </remarks>
public interface IEventlet
{
    /// <summary>
    /// Subscribes this Eventlet to the given <see cref="IEventBus"/>.
    /// </summary>
    /// <param name="bus">Event bus to subscribe to.</param>
    void Subscribe(IEventBus bus);

    /// <summary>
    /// Provides metadata describing this Eventlet.
    /// </summary>
    EventletMetadataAttribute Metadata { get; }

    /// <summary>
    /// List of event types that this Eventlet consumes.
    /// </summary>
    Type[] Consumes { get; }

    /// <summary>
    /// List of event types that this Eventlet produces.
    /// </summary>
    Type[] Produces { get; }
}