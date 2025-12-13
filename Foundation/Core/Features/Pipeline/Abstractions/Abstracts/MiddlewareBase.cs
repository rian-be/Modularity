using Core.Features.Pipeline.Abstractions.Middleware;

namespace Core.Features.Pipeline.Abstractions.Abstracts;

/// <summary>
/// Base class for creating pipeline middleware with structured execution flow and exception handling.
/// </summary>
/// <typeparam name="TContext">The type of the pipeline execution context.</typeparam>
/// <remarks>
/// <list type="bullet">
/// <item>Implements <see cref="IMiddleware{TContext}"/> to integrate with the pipeline.</item>
/// <item>Provides pre- and post-execution hooks via <see cref="BeforeAsync"/> and <see cref="AfterAsync"/>.</item>
/// <item>Enforces a single call to the next middleware through <c>SafeNext</c> to prevent multiple invocations.</item>
/// <item>Supports exception handling with <see cref="OnExceptionAsync"/> allowing derived classes to suppress or handle exceptions.</item>
/// <item>Derived classes must implement <see cref="InvokeCoreAsync"/> to provide core middleware logic.</item>
/// </list>
/// </remarks>
public abstract class MiddlewareBase<TContext> : IMiddleware<TContext>
{
    /// <inheritdoc />
    public async Task InvokeAsync(TContext context, Func<Task> next)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        ArgumentNullException.ThrowIfNull(next);

        var nextCalled = false;

        try
        {
            await BeforeAsync(context);
            await InvokeCoreAsync(context, SafeNext);
            await AfterAsync(context);
        }
        catch (Exception ex)
        {
            if (!await OnExceptionAsync(context, ex))
                throw;
        }

        return;

        async Task SafeNext()
        {
            if (nextCalled)
                throw new InvalidOperationException(
                    $"Middleware '{GetType().Name}' attempted to call next() more than once.");

            nextCalled = true;
            await next();
        }
    }

    /// <summary>
    /// Called before the middleware executes.
    /// </summary>
    /// <param name="context">The pipeline execution context.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected virtual Task BeforeAsync(TContext context) =>
        Task.CompletedTask;

    /// <summary>
    /// Core middleware logic to be implemented by derived classes.
    /// </summary>
    /// <param name="context">The pipeline execution context.</param>
    /// <param name="next">Delegate to invoke the next middleware in the pipeline.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected abstract Task InvokeCoreAsync(
        TContext context,
        Func<Task> next);

    /// <summary>
    /// Called after the middleware executes.
    /// </summary>
    /// <param name="context">The pipeline execution context.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected virtual Task AfterAsync(TContext context) =>
        Task.CompletedTask;

    /// <summary>
    /// Handles exceptions thrown during middleware execution.
    /// </summary>
    /// <param name="context">The pipeline execution context.</param>
    /// <param name="exception">The exception that was thrown.</param>
    /// <returns>
    /// True if the exception was handled and should not propagate; otherwise false.
    /// </returns>
    protected virtual Task<bool> OnExceptionAsync(
        TContext context,
        Exception exception) =>
        Task.FromResult(false);
}
