namespace Core.Features.Pipeline.Abstractions;

/// <summary>
/// Executes a configured middleware pipeline for a specific context.
/// </summary>
/// <typeparam name="TContext">The type of context the pipeline operates on.</typeparam>
/// <remarks>
/// <list type="bullet">
/// <item>Responsible for running all registered middleware in the order defined by <see cref="IPipelineBuilder{TContext}"/>.</item>
/// <item>Supports asynchronous execution and ensures proper flow through the pipeline.</item>
/// <item>Typically invoked by a host or runtime to process a request, event, or command.</item>
/// </list>
/// </remarks>
public interface IPipelineExecutor<in TContext>
{
    /// <summary>
    /// Executes the pipeline using the provided context.
    /// </summary>
    /// <param name="context">The context instance that the pipeline operates on.</param>
    /// <returns>A <see cref="Task"/> that completes when the pipeline has finished processing.</returns>
    Task ExecuteAsync(TContext context);
}