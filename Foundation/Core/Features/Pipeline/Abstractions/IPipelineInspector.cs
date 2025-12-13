using Core.Features.Pipeline.Abstractions.Middleware;

namespace Core.Features.Pipeline.Abstractions;

/// <summary>
/// Provides inspection and management capabilities for a middleware pipeline.
/// </summary>
/// <typeparam name="TContext">The type of context the pipeline operates on.</typeparam>
/// <remarks>
/// <list type="bullet">
/// <item>Allows retrieval of all registered middleware in execution order.</item>
/// <item>Supports removal of middleware by predicate.</item>
/// <item>Enables clearing of the entire pipeline.</item>
/// <item>Useful for diagnostics, testing, or dynamic pipeline modification at runtime.</item>
/// </list>
/// </remarks>
public interface IPipelineInspector<in TContext>
{
    /// <summary>
    /// Returns a read-only list of all middleware currently registered in the pipeline.
    /// </summary>
    IReadOnlyList<IMiddleware<TContext>> GetMiddlewares();

    IReadOnlyList<IMiddlewareDescriptor> GetDescriptors();
    
    /// <summary>
    /// Removes middleware matching the specified predicate.
    /// </summary>
    /// <param name="predicate">Function to select middleware to remove.</param>
    /// <returns>True if any middleware were removed; otherwise, false.</returns>
    bool Remove(Predicate<IMiddleware<TContext>> predicate);

    /// <summary>
    /// Clears all middleware from the pipeline.
    /// </summary>
    void Clear();
}