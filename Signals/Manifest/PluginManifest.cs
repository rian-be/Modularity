using System.Text.Json.Serialization;
using Signals.Interfaces;

namespace Signals.Manifest;

/// <summary>
/// Represents the manifest of a plugin, containing metadata, entry point, and dependency information.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Used by plugin loaders to identify, load, and activate plugins dynamically.</item>
/// <item>Contains metadata such as <see cref="Id"/>, <see cref="Name"/>, <see cref="Version"/>, <see cref="Author"/>, and <see cref="Description"/>.</item>
/// <item><see cref="EntryPoint"/> specifies the fully-qualified type name of the class implementing <see cref="ISignalModule"/>.</item>
/// <item><see cref="ApiVersion"/> can be used to enforce compatibility with the host system.</item>
/// <item><see cref="Dependencies"/> lists other plugins that must be loaded first to satisfy runtime requirements.</item>
/// <item>Supports serialization/deserialization via JSON for manifest files.</item>
/// </list>
/// </remarks>
public sealed class PluginManifest
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("version")]
    public required string Version { get; init; }

    [JsonPropertyName("entryPoint")]
    public required string EntryPoint { get; init; }

    [JsonPropertyName("author")]
    public string? Author { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("apiVersion")]
    public string? ApiVersion { get; init; }

    [JsonPropertyName("dependencies")]
    public PluginDependency[] Dependencies { get; init; } = [];
}