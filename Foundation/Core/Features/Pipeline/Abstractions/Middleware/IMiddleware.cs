namespace Core.Features.Pipeline.Abstractions.Middleware;

/// <summary>
/// Represents a single middleware component within a pipeline execution flow.
/// </summary>
/// <typeparam name="TContext">The type of context passed through the pipeline.</typeparam>
/// <remarks>
/// <list type="bullet">
/// <item>Middleware participates in a chain-of-responsibility pipeline model.</item>
/// <item>Each middleware is responsible for invoking the <paramref>
///         <name>next</name>
///     </paramref>
///     delegate to continue execution.</item>
/// <item>Middleware may execute logic before and/or after invoking the next component.</item>
/// <item>Failure to call <paramref>
///         <name>next</name>
///     </paramref>
///     will short-circuit the pipeline.</item>
/// </list>
/// </remarks>
public interface IMiddleware<in TContext>
{
    /// <summary>
    /// Executes middleware logic and optionally invokes the next middleware in the pipeline.
    /// </summary>
    /// <param name="context">The current pipeline context.</param>
    /// <param name="next">
    /// Delegate that triggers execution of the next middleware in the pipeline.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous execution of the middleware.
    /// </returns>
    Task InvokeAsync(TContext context, Func<Task> next);
}