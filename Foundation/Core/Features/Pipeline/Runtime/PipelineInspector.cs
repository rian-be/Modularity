using Core.Features.Pipeline.Abstractions;

namespace Core.Features.Pipeline.Runtime;

/// <summary>
/// Provides inspection and manipulation capabilities for a configured middleware pipeline.
/// </summary>
/// <typeparam name="TContext">The type of context the pipeline operates on.</typeparam>
/// <remarks>
/// <list type="bullet">
/// <item>Allows retrieval of all registered middlewares in the pipeline.</item>
/// <item>Supports removal of middlewares by predicate.</item>
/// <item>Supports clearing the entire pipeline.</item>
/// <item>Implements <see cref="IPipelineInspector{TContext}"/> for standard inspection interface.</item>
/// </list>
/// </remarks>
public class PipelineInspector<TContext>(List<Func<TContext, Func<Task>, Task>> middlewares)
    : IPipelineInspector<TContext>
{
    /// <inheritdoc />
    public IReadOnlyList<IMiddleware<TContext>> GetMiddlewares() =>
        middlewares
            .Select(mw => mw.Target)
            .OfType<IMiddleware<TContext>>()
            .ToArray();
    
    /// <inheritdoc />
    public bool Remove(Func<IMiddleware<TContext>, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        var toRemove = middlewares
            .Where(mw => mw.Target is IMiddleware<TContext> imw && predicate(imw))
            .ToList();

        foreach (var mw in toRemove)
            middlewares.Remove(mw);

        return toRemove.Count > 0;
    }
    
    /// <inheritdoc />
    public void Clear() => middlewares.Clear();
}