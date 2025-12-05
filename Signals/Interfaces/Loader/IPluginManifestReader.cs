using Signals.Manifest;

namespace Signals.Interfaces.Loader;

/// <summary>
/// Defines a service responsible for reading plugin manifests from files.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Encapsulates the logic of deserializing <see cref="PluginManifest"/> from a given file path.</item>
/// <item>Allows plugins to provide metadata, dependencies, and configuration through manifests.</item>
/// <item>Decouples manifest reading from plugin activation and assembly loading.</item>
/// </list>
/// </remarks>
public interface IPluginManifestReader
{
    /// <summary>
    /// Reads a <see cref="PluginManifest"/> from the specified file path.
    /// </summary>
    /// <param name="manifestPath">The full path to the plugin manifest file.</param>
    /// <returns>The deserialized <see cref="PluginManifest"/>.</returns>
    PluginManifest Read(string manifestPath);
}