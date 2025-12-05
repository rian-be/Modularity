using System.Reflection;
using Signals.Interfaces;
using Signals.Interfaces.Loader;
using Signals.Manifest;

namespace Signals.Loader;

/// <summary>
/// Default implementation of <see cref="IPluginActivator"/> that instantiates a signal module from a plugin assembly.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Uses the <see cref="PluginManifest.EntryPoint"/> to locate the type implementing <see cref="ISignalModule"/>.</item>
/// <item>Creates an instance using <see>
///         <cref>Activator.CreateInstance</cref>
///     </see>
///     .</item>
/// <item>Throws exceptions if the entry point type is not found or cannot be instantiated.</item>
/// <item>Supports standard plugin activation with default constructors.</item>
/// </list>
/// </remarks>
public sealed class DefaultPluginActivator : IPluginActivator
{
    /// <inheritdoc />
    public ISignalModule Create(Assembly asm, PluginManifest manifest)
    {
        var type = asm.GetType(manifest.EntryPoint, throwOnError: true);
        return (ISignalModule)Activator.CreateInstance(type!)!;
    }
}