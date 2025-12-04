using Signals.Delegates;
using Signals.Interfaces;

namespace Signals.Middleware;

/// <summary>
/// Middleware that logs events as they pass through the pipeline.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Logs the type of each <see cref="IEvent"/> before and after it is handled by downstream handlers.</item>
/// <item>Useful for debugging, tracing, or monitoring event flow in the system.</item>
/// <item>Does not modify events or affect pipeline behavior beyond logging.</item>
/// <item>Can be combined with other <see cref="IEventMiddleware"/> to form a processing pipeline.</item>
/// <item>Example log output: <c>[EventBus] -> EventTypeName</c> before and <c>[EventBus] <- EventTypeName</c> after handling.</item>
/// </list>
/// </remarks>
public sealed class LoggingMiddleware : IEventMiddleware
{
    /// <inheritdoc />
    public async Task InvokeAsync(
        IEvent evt,
        EventContext ctx,
        EventDelegate next)
    {
        /*
        Console.WriteLine(
            $"[EventBus] {evt.GetType().Name} | " +
            $"transparent={ctx.TraceParent} | " +
            $"corr={ctx.CorrelationId} | req={ctx.RequestId} | user={ctx.UserId}"
        );
        */
        Console.WriteLine($"[EventBus] -> {evt.GetType().Name}");
        await next(evt, ctx);
        Console.WriteLine($"[EventBus] <- {evt.GetType().Name}");
    }
}