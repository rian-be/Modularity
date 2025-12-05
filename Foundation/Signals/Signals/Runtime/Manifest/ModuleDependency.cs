using System.Text.Json.Serialization;

namespace Signals.Runtime.Manifest;

/// <summary>
/// Represents a dependency of a module, including the required module Id and optional version.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Used by <see cref="ModuleManifest"/> to declare dependencies on other modules.</item>
/// <item>The <see cref="Id"/> is required and uniquely identifies the dependent module.</item>
/// <item>The <see cref="Version"/> is optional and can be used to specify a minimal or exact version requirement.</item>
/// <item>Supports dependency resolution and sorting when loading modules dynamically.</item>
/// </list>
/// </remarks>
public sealed class ModuleDependency
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }
    
    [JsonPropertyName("version")]
    public string? Version { get; init; }
}
