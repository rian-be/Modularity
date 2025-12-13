namespace Core.Features.Pipeline.Abstractions.Middleware.Attributes;

/// <summary>
/// Attribute used to provide descriptive metadata for a middleware class.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Specifies the name of the middleware via <see cref="Name"/>.</item>
/// <item>Defines the kind of middleware (<see cref="Kind"/>) such as Standard, Conditional, Branch, Terminal, or Diagnostic.</item>
/// <item>Indicates whether the middleware is terminal (<see cref="IsTerminal"/>).</item>
/// <item>Indicates whether the middleware is conditional (<see cref="IsConditional"/>).</item>
/// <item>Allows attaching arbitrary key-value metadata (<see cref="Metadata"/>).</item>
/// <item>This attribute can only be applied to classes (<see cref="AttributeTargets.Class"/>).</item>
/// </list>
/// </remarks>
[AttributeUsage(AttributeTargets.Class)]
public class MiddlewareDescriptorAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the human-readable name of the middleware.
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Gets or sets the kind of middleware.
    /// </summary>
    public MiddlewareKind Kind { get; init; } = MiddlewareKind.Standard;

    /// <summary>
    /// Gets or sets a value indicating whether this middleware is terminal.
    /// </summary>
    public bool IsTerminal { get; init; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether this middleware is conditional.
    /// </summary>
    public bool IsConditional { get; init; } = false;

    /// <summary>
    /// Gets or sets custom metadata associated with the middleware.
    /// </summary>
    public Dictionary<string, object?> Metadata { get; init; } = new();
}