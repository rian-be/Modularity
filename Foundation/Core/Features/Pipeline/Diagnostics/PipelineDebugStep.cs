namespace Core.Features.Pipeline.Diagnostics;

/// <summary>
/// Represents a single execution step of a middleware within a pipeline for debugging purposes.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Records the middleware instance that was executed.</item>
/// <item>Keeps track of the start time and total duration of execution.</item>
/// <item>Indicates whether the middleware called <c>next()</c> in the pipeline.</item>
/// <item>Used in combination with <see cref="PipelineDebugContext"/> to trace pipeline execution flow.</item>
/// </list>
/// </remarks>
public class PipelineDebugStep
{
    /// <summary>
    /// The middleware instance associated with this step.
    /// </summary>
    public object Middleware { get; set; } = null!;

    /// <summary>
    /// The UTC timestamp when this step began execution.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// The total duration of this step, calculated when <see cref="Complete"/> is called.
    /// </summary>
    public TimeSpan? Duration { get; set; }

    /// <summary>
    /// Indicates whether the middleware called <c>next()</c> to continue pipeline execution.
    /// </summary>
    public bool NextCalled { get; set; }

    /// <summary>
    /// Marks this step as complete and calculates the duration.
    /// </summary>
    public void Complete()
    {
        Duration = DateTime.UtcNow - StartTime;
    }
}