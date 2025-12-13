using Core.Features.Pipeline.Abstractions;
using Core.Features.Pipeline.Abstractions.Middleware;
using Core.Features.Pipeline.Middleware;

namespace Core.Features.Pipeline.Runtime;

/// <summary>
/// Builds and configures a middleware pipeline for a specific <typeparamref name="TContext"/> type.
/// </summary>
/// <typeparam name="TContext">The type of context that the pipeline operates on.</typeparam>
/// <remarks>
/// <list type="bullet">
/// <item>Maintains an ordered collection of middleware components.</item>
/// <item>Supports adding middleware at the end or beginning of the pipeline.</item>
/// <item>Allows inserting middleware before or after a specific middleware using predicates.</item>
/// <item>Supports conditional execution via <see cref="UseWhen"/>.</item>
/// <item>Supports branching pipelines via <see cref="UseBranch"/>.</item>
/// <item>Designed for dynamic runtime pipeline configuration.</item>
/// </list>
/// </remarks>
public class PipelineBuilder<TContext> : IPipelineBuilder<TContext>
{
    private readonly List<IMiddleware<TContext>> _middlewares = [];

    public IEnumerable<IMiddleware<TContext>> Middlewares => _middlewares;
   
    /// <inheritdoc />
    public void Use(IMiddleware<TContext> middleware) => _middlewares.Add(middleware);

    /// <inheritdoc />
    public void UseFirst(IMiddleware<TContext> middleware) => _middlewares.Insert(0, middleware);
   
    /// <inheritdoc />
    public void UseAfter(Predicate<IMiddleware<TContext>> predicate, IMiddleware<TContext> middleware)
    {
        var index = _middlewares.FindLastIndex(predicate);
        _middlewares.Insert(index >= 0 ? index + 1 : _middlewares.Count, middleware);
    }

    /// <inheritdoc />
    public void UseBefore(Predicate<IMiddleware<TContext>> predicate, IMiddleware<TContext> middleware)
    {
        var index = _middlewares.FindIndex(predicate);
        _middlewares.Insert(index >= 0 ? index : 0, middleware);
    }
    
    /// <inheritdoc />
    public void UseWhen(Func<TContext, bool> condition, IMiddleware<TContext> middleware)
    {
        Use(new ConditionalMiddleware<TContext>(condition, middleware));
    }

    /// <inheritdoc />
    public void UseBranch(Func<TContext, bool> condition, Action<IPipelineBuilder<TContext>> configurePipeline)
    {
        var branchBuilder = new PipelineBuilder<TContext>();
        configurePipeline(branchBuilder);
        Use(new BranchMiddleware<TContext>(condition, branchBuilder));
    }
}
