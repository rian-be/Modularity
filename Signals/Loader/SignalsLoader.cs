using System.Reflection;
using Signals.Interfaces;

namespace Signals.Loader;

/// <summary>
/// Responsible for dynamically loading and unloading signal modules (plugins) at runtime.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Loads assemblies from DLL paths and instantiates types implementing <see cref="ISignalModule"/>.</item>
/// <item>Registers the module's signals with a given <see cref="IEventBus"/> upon loading.</item>
/// <item>Supports unloading plugins by calling <see cref="ISignalModule.UnregisterSignals"/>.</item>
/// <item>Allows querying event types defined inside loaded plugins via <see cref="GetPluginEventType"/>.</item>
/// <item>Keeps track of loaded plugins internally to prevent double-loading or invalid unloads.</item>
/// <item>Throws descriptive exceptions if requested types or modules are not found or incompatible.</item>
/// </list>
/// <para>
/// Typically used in systems where event handling logic is extended via external assemblies,
/// allowing modular and hot-swappable event processing components.
/// </para>
/// </remarks>
public class SignalsLoader(IEventBus bus)
{
    private readonly Dictionary<string, (Assembly asm, object module)> _loaded = new();

    public void LoadPlugin(string dllPath)
    {
        var asm = Assembly.LoadFrom(dllPath);

        foreach (var type in asm.GetTypes())
        {
            if (typeof(ISignalModule).IsAssignableFrom(type) && !type.IsInterface)
            {
                var module = Activator.CreateInstance(type)!;
                ((ISignalModule)module).RegisterSignals(bus);
                _loaded[dllPath] = (asm, module);
                Console.WriteLine($"Plugin loaded: {type.Name}");
                return;
            }
        }
    }

    public void UnloadPlugin(string dllPath)
    {
        if (!_loaded.TryGetValue(dllPath, out var entry)) return;
        ((ISignalModule)entry.module).UnregisterSignals(bus);
        _loaded.Remove(dllPath);
        Console.WriteLine($"Plugin unloaded: {dllPath}");
    }
    
    public Type GetPluginEventType(string dllPath, string eventTypeName)
    {
        if (!_loaded.TryGetValue(dllPath, out var entry))
            throw new InvalidOperationException("Plugin not loaded");

        var type = entry.asm.GetType(eventTypeName, throwOnError: false);
        if (type == null)
            throw new InvalidOperationException($"Type '{eventTypeName}' not found in plugin");

        if (!typeof(IEvent).IsAssignableFrom(type))
            throw new InvalidOperationException($"Type '{eventTypeName}' does not implement IEvent");

        return type;
    }
}