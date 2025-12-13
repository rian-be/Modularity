namespace Core.Features.Pipeline.Diagnostics;

/// <summary>
/// Provides an ambient, async-safe debug scope for pipeline execution.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Uses <see cref="AsyncLocal{T}"/> to track the current <see cref="PipelineDebugContext"/> per async flow.</item>
/// <item>Allows middleware to record execution steps in <see cref="PipelineDebugContext"/>.</item>
/// <item>Typical usage involves creating a scope at the start of pipeline execution and disposing it at the end.</item>
/// </list>
/// </remarks>
public static class PipelineDebugScope
{
    private static readonly AsyncLocal<PipelineDebugContext?> CurrentContext = new();

    /// <summary>
    /// Gets the current <see cref="PipelineDebugContext"/> for the current async flow.
    /// Returns <c>null</c> if no scope is active.
    /// </summary>
    public static PipelineDebugContext? Current => CurrentContext.Value;

    /// <summary>
    /// Begins a new debug scope and sets the <see cref="Current"/> context.
    /// </summary>
    /// <param name="context">Outputs the newly created <see cref="PipelineDebugContext"/>.</param>
    /// <returns>An <see cref="IDisposable"/> that ends the scope when disposed.</returns>
    public static IDisposable Begin(out PipelineDebugContext context)
    {
        context = new PipelineDebugContext();
        CurrentContext.Value = context;

        return new Scope();
    }

    private sealed class Scope : IDisposable
    {
        /// <summary>
        /// Ends the current debug scope by clearing the <see cref="Current"/> context.
        /// </summary>
        public void Dispose() => CurrentContext.Value = null;
    }
}