using System.Collections.Concurrent;
using Signals.Interfaces;

namespace Signals.Metadata;

/// <summary>
/// Represents a collection of <see cref="HandlerWrapper"/> instances for a specific event type.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Stores handlers in a thread-safe <see cref="ConcurrentBag{T}"/>.</item>
/// <item>Supports adding new handlers, unsubscribing, and taking snapshots of applicable handlers.</item>
/// <item>Automatically handles filtering and priority ordering of handlers.</item>
/// <item>Supports "once-only" handlers via <see cref="RemoveOnceHandlers"/>.</item>
/// </list>
/// </remarks>
public sealed class HandlerCollection
{
    private readonly ConcurrentBag<HandlerWrapper> _handlers = [];

    public void Add(HandlerWrapper wrapper) => _handlers.Add(wrapper);

    public HandlerWrapper[] GetSnapshot(IEvent evt)
    {
        return _handlers
            .Where(h => h.Filter is null || h.Filter(evt))
            .OrderByDescending(h => h.Priority)
            .ToArray();
    }

    public void RemoveOnceHandlers()
    {
        var remaining = _handlers.Where(h => !h.Once).ToArray();
        _handlers.Clear();
        foreach (var h in remaining) _handlers.Add(h);
    }

    public void Unsubscribe(Func<IEvent, Task> handler)
    {
        var remaining = _handlers.Where(h => h.Handler != handler).ToArray();
        _handlers.Clear();
        foreach (var h in remaining) _handlers.Add(h);
    }

    public bool IsEmpty => _handlers.IsEmpty;
}