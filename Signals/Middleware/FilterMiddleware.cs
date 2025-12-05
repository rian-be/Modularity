using Signals.Delegates;
using Signals.Interfaces;

namespace Signals.Middleware;

/// <summary>
/// Middleware that filters events based on a predicate.
/// </summary>
/// <typeparam name="TEvent">Type of event to filter. Must implement <see cref="IEvent"/>.</typeparam>
/// <remarks>
/// <list type="bullet">
/// <item>Executes only for events of type <typeparamref name="TEvent"/>.</item>
/// <item>Skips events that do not satisfy the provided <c>predicate</c>, preventing further propagation.</item>
/// <item>Passes events that satisfy the predicate to the next middleware or final handler.</item>
/// <item>Useful for conditional handling, event scoping, or selective subscription logic.</item>
/// <item>Can be chained with other <see cref="IEventMiddleware"/> components in the pipeline.</item>
/// </list>
/// </remarks>
public sealed class FilterMiddleware : IEventMiddleware
{
    private readonly Func<IEvent, bool> _predicate;

    public FilterMiddleware(Func<IEvent, bool> predicate)
    {
        _predicate = predicate;
    }

    public async Task InvokeAsync(IEvent evt, EventContext ctx, EventDelegate next)
    {
        if (!_predicate(evt))
            return;

        await next(evt, ctx);
    }
}
