using Core.Features.Pipeline.Abstractions;
using Core.Features.Pipeline.Abstractions.Middleware;
using Core.Features.Pipeline.Diagnostics;

namespace Core.Features.Pipeline.Runtime;

/// <summary>
/// Executes a configured middleware pipeline for a specific <typeparamref name="TContext"/> instance.
/// </summary>
/// <typeparam name="TContext">The type of context the pipeline operates on.</typeparam>
/// <remarks>
/// <list type="bullet">
/// <item>Consumes a <see cref="PipelineBuilder{TContext}"/> and executes its middlewares in order.</item>
/// <item>Supports asynchronous middleware execution using a continuation delegate (<c>Next</c>).</item>
/// <item>Implements <see cref="IPipelineExecutor{TContext}"/> for standard pipeline execution.</item>
/// <item>Optionally wraps middlewares with <see cref="PipelineDebuggerMiddleware{TContext}"/> to collect execution diagnostics.</item>
/// <item>Each middleware can decide whether to invoke the next middleware in the chain, enabling short-circuiting.</item>
/// </list>
/// </remarks>
public class PipelineExecutor<TContext> : IPipelineExecutor<TContext>
{
    private readonly IList<IMiddleware<TContext>> _middlewares;

    /// <summary>
    /// Initializes a new instance of <see cref="PipelineExecutor{TContext}"/>.
    /// </summary>
    /// <param name="builder">The pipeline builder containing the configured middleware.</param>
    /// <param name="enableDebug">
    /// If true, each middleware is wrapped in <see cref="PipelineDebuggerMiddleware{TContext}"/>
    /// to track execution steps and timing.
    /// </param>
    public PipelineExecutor(PipelineBuilder<TContext> builder, bool enableDebug = false)
    {
        _middlewares = builder.Middlewares
            .Select(mw => enableDebug ? new PipelineDebuggerMiddleware<TContext>(mw) : mw)
            .ToList();
    }

    /// <summary>
    /// Recursively executes the middleware pipeline starting from the specified index.
    /// </summary>
    /// <param name="index">The current middleware index.</param>
    /// <param name="context">The context instance for this pipeline execution.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private Task ExecuteAsync(int index, TContext context)
    {
        if (index >= _middlewares.Count)
            return Task.CompletedTask;

        var middleware = _middlewares[index];
        return middleware.InvokeAsync(context, () => ExecuteAsync(index + 1, context));
    }

    /// <inheritdoc />
    public Task ExecuteAsync(TContext context) =>
        ExecuteAsync(0, context);
}
