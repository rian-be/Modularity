using Signals.Core.Events;

namespace Signals.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class HandlesEventAttribute : Attribute
{
    public Type EventType { get; }
    
    public HandlesEventAttribute(Type eventType)
    {
        if (!typeof(IEvent).IsAssignableFrom(eventType))
            throw new ArgumentException("EventType must implement IEvent");
        EventType = eventType;
    }
}