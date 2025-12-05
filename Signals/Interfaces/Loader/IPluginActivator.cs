using System.Reflection;
using Signals.Loader;
using Signals.Manifest;

namespace Signals.Interfaces.Loader;

/// <summary>
/// Defines a factory responsible for instantiating signal modules from assemblies and their manifests.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Used to decouple plugin loading logic from instantiation.</item>
/// <item>Allows creating <see cref="ISignalModule"/> instances dynamically based on assembly content and metadata.</item>
/// <item>Supports extensible plugin systems where modules may require additional configuration from <see cref="PluginManifest"/>.</item>
/// <item>Enables different activation strategies (e.g., default constructor, DI container, custom factory) without modifying the loader.</item>
/// </list>
/// </remarks>
public interface IPluginActivator
{
    /// <summary>
    /// Creates an <see cref="ISignalModule"/> instance from the specified assembly and manifest.
    /// </summary>
    /// <param name="asm">The assembly containing the module type.</param>
    /// <param name="manifest">The plugin manifest containing metadata and configuration.</param>
    /// <returns>An instantiated <see cref="ISignalModule"/>.</returns>
    ISignalModule Create(Assembly asm, PluginManifest manifest);
}