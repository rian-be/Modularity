namespace Signals.Attributes;

/// <summary>
/// Declares an event that an Eventlet produces.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Can be applied multiple times to the same class to declare multiple produced events.</item>
/// <item>Eventlet loader reads this attribute to know which events can be emitted by this Eventlet.</item>
/// <item>Helps in building event dependency graphs and detecting possible cycles or missing consumers.</item>
/// </list>
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class ProducesEventAttribute(Type eventType) : Attribute
{
    public Type EventType { get; } = eventType;
}