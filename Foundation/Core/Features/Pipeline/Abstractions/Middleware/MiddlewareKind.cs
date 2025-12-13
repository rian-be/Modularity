namespace Core.Features.Pipeline.Abstractions.Middleware;

/// <summary>
/// Defines the semantic category of a middleware component within a pipeline.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Used for runtime inspection, diagnostics, and pipeline visualization.</item>
/// <item>Helps tooling and debuggers reason about execution flow.</item>
/// <item>Does not directly affect execution unless explicitly interpreted by the runtime.</item>
/// </list>
/// </remarks>
public enum MiddlewareKind
{
    /// <summary>
    /// Represents a standard middleware that participates in the normal
    /// request flow and is expected to call <c>next()</c>.
    /// </summary>
    Standard,

    /// <summary>
    /// Represents a middleware that executes conditionally based on runtime context.
    /// </summary>
    /// <remarks>
    /// Typically created via constructs such as <c>UseWhen</c> or guarded execution.
    /// </remarks>
    Conditional,

    /// <summary>
    /// Represents a branching middleware that may execute an independent
    /// sub-pipeline.
    /// </summary>
    /// <remarks>
    /// Branch middleware does not replace the main pipeline but augments it
    /// with additional execution paths.
    /// </remarks>
    Branch,

    /// <summary>
    /// Represents a terminal middleware that ends pipeline execution.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>Terminal middleware is expected not to call <c>next()</c>.</item>
    /// <item>Usually represents final handlers or dispatch operations.</item>
    /// </list>
    /// </remarks>
    Terminal,

    /// <summary>
    /// Represents a diagnostic or infrastructure middleware.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>Used for logging, tracing, metrics, or debugging.</item>
    /// <item>Should be side effect safe and transparent to business logic.</item>
    /// </list>
    /// </remarks>
    Diagnostic
}