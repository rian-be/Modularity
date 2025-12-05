using System.Text.Json;
using Signals.Runtime.Interfaces;
using Signals.Runtime.Manifest;

namespace Signals.Runtime.Loader;

/// <summary>
/// Implementation of <see cref="IModuleManifestReader"/> that reads plugin manifests from JSON files.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Reads the content of the specified manifest file as JSON.</item>
/// <item>Deserializes the JSON into a <see cref="ModuleManifest"/> instance.</item>
/// <item>Throws <see cref="InvalidOperationException"/> if deserialization fails or the manifest is invalid.</item>
/// <item>Supports standard JSON manifest format for plugins.</item>
/// </list>
/// </remarks>
public sealed class JsonModuleManifestReader : IModuleManifestReader
{
    /// <inheritdoc />
    public ModuleManifest Read(string manifestPath)
    {
        var json = File.ReadAllText(manifestPath);
        return JsonSerializer.Deserialize<ModuleManifest>(json)!
               ?? throw new InvalidOperationException("Invalid plugin manifest.");
    }
}