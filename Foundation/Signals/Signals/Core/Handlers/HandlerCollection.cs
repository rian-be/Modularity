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
    
    private readonly ConcurrentDictionary<long, HandlerWrapper> _handlers = new();

    public void Add(HandlerWrapper wrapper)
        => _handlers.TryAdd(wrapper.Id, wrapper);

    public HandlerWrapper[] GetSnapshot(IEvent evt)
    {
        return _handlers.Values
            .Where(h => h.Filter == null || h.Filter(evt))
            .OrderByDescending(h => h.Priority)
            .ToArray();
    }
    
    public HandlerWrapper? GetFirst()
    {
        HandlerWrapper? best = null;

        foreach (var h in _handlers.Values)
        {
            if (best == null || h.Priority > best.Priority)
                best = h;
        }

        return best;
    }
    
    public bool RemoveById(long id)
        => _handlers.TryRemove(id, out _);
    
    public void RemoveOnceHandlers()
    {
        foreach (var h in _handlers.Values)
        {
            if (h.Once)
                _handlers.TryRemove(h.Id, out _);
        }
    }

    public bool IsEmpty => _handlers.IsEmpty;
}