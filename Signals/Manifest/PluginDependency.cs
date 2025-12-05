using System.Text.Json.Serialization;

namespace Signals.Manifest;

/// <summary>
/// Represents a dependency of a plugin, including the required plugin Id and optional version.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Used by <see cref="PluginManifest"/> to declare dependencies on other plugins.</item>
/// <item>The <see cref="Id"/> is required and uniquely identifies the dependent plugin.</item>
/// <item>The <see cref="Version"/> is optional and can be used to specify a minimal or exact version requirement.</item>
/// <item>Supports dependency resolution and sorting when loading plugins dynamically.</item>
/// </list>
/// </remarks>
public sealed class PluginDependency
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }
    
    [JsonPropertyName("version")]
    public string? Version { get; init; }
}
