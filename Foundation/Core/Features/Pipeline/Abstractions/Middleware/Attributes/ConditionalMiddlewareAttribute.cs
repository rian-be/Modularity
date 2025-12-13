namespace Core.Features.Pipeline.Abstractions.Middleware.Attributes;

/// <summary>
/// Marks a middleware class as conditional, indicating that it should only execute
/// when a specified runtime condition is met.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Applied to middleware classes to signal that execution depends on a condition.</item>
/// <item>Does not inherit to derived classes (<see>
///         <cref>Inherited</cref>
///     </see>
///     = false).</item>
/// <item>Can include an optional <see cref="Description"/> to document the condition.</item>
/// </list>
/// </remarks>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class ConditionalMiddlewareAttribute : Attribute
{
    /// <summary>
    /// Optional description of the condition controlling middleware execution.
    /// </summary>
    public string? Description { get; init; }
}