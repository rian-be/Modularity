using Signals.Delegates;

namespace Signals.Interfaces;

/// <summary>
/// Represents a middleware component in the event processing pipeline.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Executed for every published <see cref="IEvent"/> before it reaches the final handlers.</item>
/// <item>Allows interception, modification, filtering, or logging of events.</item>
/// <item>Controls the execution flow by invoking the next middleware via <paramref name="next"/>.</item>
/// <item>Multiple middlewares can be chained to form a processing pipeline.</item>
/// <item>Typical use cases include validation, metrics, tracing, retries, filtering, or transaction scopes.</item>
/// <item>EventContext provides additional metadata, tracing, or pipeline control for the event.</item>
/// </list>
/// </remarks>
public interface IEventMiddleware
{
    /// <summary>
    /// Executes middleware logic for the given event and optionally passes control to the next middleware.
    /// </summary>
    /// <param name="evt">The event being processed.</param>
    /// <param name="ctx">Context providing metadata, tracing, or additional control for the event.</param>
    /// <param name="next">Delegate to invoke the next middleware or final event handler in the pipeline.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task InvokeAsync(IEvent evt, EventContext ctx, EventDelegate next);
}