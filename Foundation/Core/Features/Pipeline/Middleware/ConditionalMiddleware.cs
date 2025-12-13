using Core.Features.Pipeline.Abstractions.Middleware;

namespace Core.Features.Pipeline.Middleware;

/// <summary>
/// Middleware that conditionally executes an inner middleware based on predicate.
/// </summary>
/// <typeparam name="TContext">The type of context the pipeline operates on.</typeparam>
/// <remarks>
/// <list type="bullet">
/// <item>Evaluates the provided <see cref="Func{TContext, Boolean}"/> predicate before invoking the inner middleware.</item>
/// <item>If the condition is true, the inner <see cref="IMiddleware{TContext}"/> is executed.</item>
/// <item>If the condition is false, the middleware skips the inner middleware and continues with the next one in the pipeline.</item>
/// <item>Useful for feature flags, scenario-specific logic, or conditional branching within a pipeline.</item>
/// </list>
/// </remarks>
public class ConditionalMiddleware<TContext>(Func<TContext, bool> condition, IMiddleware<TContext> inner)
    : IMiddleware<TContext>
{
    private readonly Func<TContext, bool> _condition = condition ?? throw new ArgumentNullException(nameof(condition));
    private readonly IMiddleware<TContext> _inner = inner ?? throw new ArgumentNullException(nameof(inner));
    
    /// <inheritdoc />
    public async Task InvokeAsync(TContext context, Func<Task> next)
    {
        if (_condition(context))
            await _inner.InvokeAsync(context, next);
        else
            await next();
    }
}