namespace Core.Features.Pipeline.Abstractions.Middleware;

/// <summary>
/// Describes a middleware component in a pipeline in a declarative,
/// runtime-inspectable form.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Provides metadata about middleware without exposing its implementation.</item>
/// <item>Used for inspection, diagnostics, debugging, and visualization of pipelines.</item>
/// <item>Allows the runtime to reason about middleware behavior (e.g. terminal, conditional).</item>
/// <item>Decouples pipeline structure analysis from concrete middleware instances.</item>
/// </list>
/// </remarks>
public interface IMiddlewareDescriptor
{
    /// <summary>
    /// Gets the concrete middleware type.
    /// </summary>
    Type MiddlewareType { get; }

    /// <summary>
    /// Gets the logical, human-readable name of the middleware.
    /// </summary>
    /// <remarks>
    /// Typically used for diagnostics, logging, and debugging tools.
    /// </remarks>
    string Name { get; }

    /// <summary>
    /// Gets the middleware classification.
    /// </summary>
    /// <remarks>
    /// Used by the pipeline runtime to understand execution semantics
    /// such as branching, terminal behavior, or infrastructure concerns.
    /// </remarks>
    MiddlewareKind Kind { get; }

    /// <summary>
    /// Gets a value indicating whether this middleware terminates the pipeline execution.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>If <c>true</c>, the middleware is expected not to call <c>next()</c>.</item>
    /// <item>Terminal middleware usually represents final handlers or dispatchers.</item>
    /// </list>
    /// </remarks>
    bool IsTerminal { get; }

    /// <summary>
    /// Gets a value indicating whether this middleware executes conditionally.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>Conditional middleware may skip execution based on runtime context.</item>
    /// <item>Includes constructs such as <c>UseWhen</c> or branch-based middleware.</item>
    /// </list>
    /// </remarks>
    bool IsConditional { get; }

    /// <summary>
    /// Gets arbitrary metadata associated with the middleware.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>Intended for tooling, diagnostics, and runtime extensions.</item>
    /// <item>Must not be relied upon for core execution semantics.</item>
    /// <item>Values should be immutable or treated as read-only.</item>
    /// </list>
    /// </remarks>
    IReadOnlyDictionary<string, object?> Metadata { get; }
}
