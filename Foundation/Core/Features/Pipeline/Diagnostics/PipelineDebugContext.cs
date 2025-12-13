namespace Core.Features.Pipeline.Diagnostics;

/// <summary>
/// Represents a debugging context for a middleware pipeline, tracking execution steps and timings.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Allows capturing and inspecting the sequence of middleware execution.</item>
/// <item>Each step records the middleware instance and timing information.</item>
/// <item>Useful for logging, profiling, and diagnosing pipeline behavior.</item>
/// </list>
/// </remarks>
public class PipelineDebugContext
{
    private readonly List<PipelineDebugStep> _steps = [];

    /// <summary>
    /// Gets the read-only list of executed pipeline steps.
    /// </summary>
    public IReadOnlyList<PipelineDebugStep> Steps => _steps;

    /// <summary>
    /// Begins tracking a new pipeline execution step.
    /// </summary>
    /// <param name="middleware">The middleware instance associated with this step.</param>
    /// <returns>The <see cref="PipelineDebugStep"/> representing the started step.</returns>
    public PipelineDebugStep BeginStep(object middleware)
    {
        var step = new PipelineDebugStep
        {
            Middleware = middleware,
            StartTime = DateTime.UtcNow
        };
        _steps.Add(step);
        return step;
    }
}