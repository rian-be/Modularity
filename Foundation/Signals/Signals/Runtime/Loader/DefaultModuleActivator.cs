using System.Reflection;
using Signals.Core.Modules;
using Signals.Runtime.Interfaces;
using Signals.Runtime.Manifest;

namespace Signals.Runtime.Loader;

/// <summary>
/// Default implementation of <see cref="IModuleActivator"/> that instantiates a signal module from a plugin assembly.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Uses the <see cref="ModuleManifest.EntryPoint"/> to locate the type implementing <see cref="ISignalModule"/>.</item>
/// <item>Creates an instance using <see>
///         <cref>Activator.CreateInstance</cref>
///     </see>
///     .</item>
/// <item>Throws exceptions if the entry point type is not found or cannot be instantiated.</item>
/// <item>Supports standard plugin activation with default constructors.</item>
/// </list>
/// </remarks>
public sealed class DefaultModuleActivator : IModuleActivator
{
    /// <inheritdoc />
    public ISignalModule Create(Assembly asm, ModuleManifest manifest)
    {
        var type = asm.GetType(manifest.EntryPoint, throwOnError: true);
        return (ISignalModule)Activator.CreateInstance(type!)!;
    }
}