using Core.Features.Pipeline.Abstractions;
using Core.Features.Pipeline.Abstractions.Middleware;
using Core.Features.Pipeline.Runtime.Providers;

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
public class PipelineInspector<TContext>(
    IEnumerable<IMiddleware<TContext>> middlewares,
    MiddlewareDescriptorProvider? descriptorProvider = null)
    : IPipelineInspector<TContext>
{
    private readonly List<IMiddleware<TContext>> _middlewares = middlewares.ToList();
    private readonly MiddlewareDescriptorProvider _descriptorProvider = descriptorProvider ?? new MiddlewareDescriptorProvider();

    public IReadOnlyList<IMiddleware<TContext>> GetMiddlewares() => _middlewares;

    /// <inheritdoc />
    public IReadOnlyList<IMiddlewareDescriptor> GetDescriptors()
    {
        return _middlewares
            .Select(mw => _descriptorProvider.GetDescriptor(mw))
            .ToList();
    }
    
    /// <inheritdoc />
    public bool Remove(Predicate<IMiddleware<TContext>> predicate)
    {
        var removed = _middlewares.RemoveAll(predicate);
        return removed > 0;
    }

    /// <inheritdoc />
    public void Clear() => _middlewares.Clear();
}