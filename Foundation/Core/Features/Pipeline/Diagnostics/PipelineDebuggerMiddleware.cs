using Core.Features.Pipeline.Abstractions.Middleware;

namespace Core.Features.Pipeline.Diagnostics;

/// <summary>
/// Middleware wrapper that integrates with the <see cref="PipelineDebugScope"/> to track
/// execution of an inner middleware for debugging purposes.
/// </summary>
/// <typeparam name="TContext">The type of the pipeline context.</typeparam>
/// <remarks>
/// <list type="bullet">
/// <item>Captures the start and completion time of the inner middleware.</item>
/// <item>Records whether <c>next()</c> was called.</item>
/// <item>Useful for profiling, diagnostics, and visualizing pipeline execution.</item>
/// <item>Should be placed around middleware you want to debug without altering behavior.</item>
/// </list>
/// </remarks>
public sealed class PipelineDebuggerMiddleware<TContext>(IMiddleware<TContext> inner ) : IMiddleware<TContext>
{
    /// <inheritdoc />
    public async Task InvokeAsync(TContext context, Func<Task> next)
    {
        var debug = PipelineDebugScope.Current;
        PipelineDebugStep? step = null;

        if (debug != null)
            step = debug.BeginStep(inner.GetType().Name);
        
        bool nextCalled = false;

        try
        {
            await inner.InvokeAsync(context, next);
            nextCalled = true;
        }
        finally
        {
            if (step != null)
            {
                step.Complete();
                step.NextCalled = nextCalled;
            }
        }
    }
}