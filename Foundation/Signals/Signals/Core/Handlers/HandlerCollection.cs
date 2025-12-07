using System.Collections.Concurrent;
using Signals.Core.Events;

namespace Signals.Core.Handlers;

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
    private readonly ConcurrentDictionary<HandlerWrapper, byte> _handlers = new();

    public void Add(HandlerWrapper wrapper) => _handlers.TryAdd(wrapper, 0);

    public HandlerWrapper[] GetSnapshot(IEvent evt)
    {
        return _handlers.Keys
            .Where(h => h.Filter == null || (h.Filter(evt)))
            .OrderByDescending(h => h.Priority)
            .ToArray();

    }

    public void RemoveOnceHandlers()
    {
        var remaining = _handlers.Keys.Where(h => !h.Once).ToArray();
        _handlers.Clear();
        foreach (var h in remaining)
            _handlers.TryAdd(h, 0);
    }

    public void Unsubscribe(Func<IEvent, Task> handler)
    {
        var remaining = _handlers.Keys.Where(h => h.Handler != handler).ToArray();
        _handlers.Clear();
        foreach (var h in remaining)
            _handlers.TryAdd(h, 0);
    }

    public bool IsEmpty => _handlers.IsEmpty;
}