namespace Signals.Attributes;

/// <summary>
/// Declares an event that an Eventlet consumes.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Can be applied multiple times to the same class to declare multiple consumed events.</item>
/// <item>Eventlet loader reads this attribute to know which event types to subscribe the Eventlet to.</item>
/// <item>The Eventlet must implement logic to handle the declared events; attribute only registers interest.</item>
/// </list>
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class ConsumesEventAttribute(Type eventType) : Attribute
{
    public Type EventType { get; } = eventType;
}