using System.Text.Json;
using Signals.Interfaces.Loader;
using Signals.Manifest;

namespace Signals.Loader;

/// <summary>
/// Implementation of <see cref="IPluginManifestReader"/> that reads plugin manifests from JSON files.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Reads the content of the specified manifest file as JSON.</item>
/// <item>Deserializes the JSON into a <see cref="PluginManifest"/> instance.</item>
/// <item>Throws <see cref="InvalidOperationException"/> if deserialization fails or the manifest is invalid.</item>
/// <item>Supports standard JSON manifest format for plugins.</item>
/// </list>
/// </remarks>
public sealed class JsonPluginManifestReader : IPluginManifestReader
{
    /// <inheritdoc />
    public PluginManifest Read(string manifestPath)
    {
        var json = File.ReadAllText(manifestPath);
        return JsonSerializer.Deserialize<PluginManifest>(json)!
               ?? throw new InvalidOperationException("Invalid plugin manifest.");
    }
}