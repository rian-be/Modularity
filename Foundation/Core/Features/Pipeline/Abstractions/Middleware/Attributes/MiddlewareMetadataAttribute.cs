namespace Core.Features.Pipeline.Abstractions.Middleware.Attributes;

/// <summary>
/// Attribute used to attach a single key-value metadata entry to a middleware class.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Can be applied multiple times to the same class (<see>
///         <cref>AllowMultiple</cref>
///     </see>
///     = true).</item>
/// <item>Does not inherit to derived classes (<see>
///         <cref>Inherited</cref>
///     </see>
///     = false).</item>
/// <item>Allows attaching arbitrary metadata to middleware for discovery, diagnostics, or runtime behavior.</item>
/// </list>
/// </remarks>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class MiddlewareMetadataAttribute(string key, object? value) : Attribute
{
    /// <summary>
    /// Gets the metadata key.
    /// </summary>
    public string Key { get; } = key;

    /// <summary>
    /// Gets the metadata value.
    /// </summary>
    public object? Value { get; } = value;
}