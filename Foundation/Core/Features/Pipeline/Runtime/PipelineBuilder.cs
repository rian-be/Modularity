using Core.Features.Pipeline.Abstractions;
using Core.Features.Pipeline.Middleware;

namespace Core.Features.Pipeline.Runtime;

/// <summary>
/// Builds a middleware pipeline for a specific <typeparamref name="TContext"/> type.
/// </summary>
/// <typeparam name="TContext">The type of context that the pipeline operates on.</typeparam>
/// <remarks>
/// <list type="bullet">
/// <item>Maintains an ordered list of middleware delegates.</item>
/// <item>Supports adding middleware at the end, beginning, before, or after other middleware.</item>
/// <item>Supports conditional execution via <see cref="UseWhen"/> and branching via <see cref="UseBranch"/>.</item>
/// <item>Designed for runtime configuration and dynamic modification of pipelines.</item>
/// </list>
/// </remarks>
public class PipelineBuilder<TContext> : IPipelineBuilder<TContext>
{
    public readonly List<Func<TContext, Func<Task>, Task>> Middlewares = [];
    
    /// <inheritdoc />
    public void Use(IMiddleware<TContext> middleware) =>
        Middlewares.Add(middleware.InvokeAsync);

    /// <inheritdoc />
    public void UseFirst(IMiddleware<TContext> middleware) =>
        Middlewares.Insert(0, middleware.InvokeAsync);

    /// <inheritdoc />
    public void UseAfter(Func<IMiddleware<TContext>, bool> predicate, IMiddleware<TContext> middleware)
    {
        var index = Middlewares
            .Select((fn, i) => (fn.Target, i))
            .Where(x => x.Target is IMiddleware<TContext> m && predicate(m))
            .Select(x => x.i)
            .LastOrDefault(-1);

        Middlewares.Insert(index >= 0 ? index + 1 : Middlewares.Count, middleware.InvokeAsync);
    }
    
    /// <inheritdoc />
    public void UseBefore(Func<IMiddleware<TContext>, bool> predicate, IMiddleware<TContext> middleware)
    {
        var index = Middlewares
            .Select((fn, i) => (fn.Target, i))
            .Where(x => x.Target is IMiddleware<TContext> m && predicate(m))
            .Select(x => x.i)
            .FirstOrDefault(0);

        Middlewares.Insert(index, middleware.InvokeAsync);
    }

    /// <inheritdoc />
    public void UseWhen(Func<TContext, bool> condition, IMiddleware<TContext> middleware) =>
        Use(new ConditionalMiddleware<TContext>(condition, middleware));

    /// <inheritdoc />
    public void UseBranch(Func<TContext, bool> condition, Action<IPipelineBuilder<TContext>> configurePipeline)
    {
        var branchBuilder = new PipelineBuilder<TContext>();
        configurePipeline(branchBuilder);
        Use(new BranchMiddleware<TContext>(condition, branchBuilder));
    }
}