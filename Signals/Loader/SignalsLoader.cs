using System.Reflection;
using Signals.Interfaces;
using Signals.Interfaces.Loader;
using Signals.Manifest;

namespace Signals.Loader;

/// <summary>
/// Responsible for loading, activating, and unloading signal plugins from assemblies.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Uses <see cref="IPluginManifestReader"/> to read plugin manifests from JSON files.</item>
/// <item>Resolves dependencies between plugins using <see cref="IPluginDependencyResolver"/>.</item>
/// <item>Loads assemblies via <see cref="IPluginAssemblyLoader"/> and activates signal modules via <see cref="IPluginActivator"/>.</item>
/// <item>Registers and unregisters signals in the provided <see cref="IEventBus"/>.</item>
/// <item>Maintains an internal collection of loaded plugins to prevent duplicate loading.</item>
/// <item>Supports loading multiple plugins from a directory and respects dependency order.</item>
/// </list>
/// </remarks>
public sealed class SignalsLoader(
    IEventBus bus,
    IPluginManifestReader manifestReader,
    IPluginAssemblyLoader assemblyLoader,
    IPluginDependencyResolver dependencyResolver,
    IPluginActivator activator)
{
    private readonly Dictionary<string, (Assembly asm, ISignalModule module, PluginManifest manifest)> _loaded = new();
    
    public void LoadFromDirectory(string directory)
    {
        var manifests = new List<(string dll, PluginManifest manifest)>();

        foreach (var manifestPath in Directory.GetFiles(directory, "signal.plugin.json", SearchOption.AllDirectories))
        {
            var manifest = manifestReader.Read(manifestPath);
            var dllPath = Path.Combine(Path.GetDirectoryName(manifestPath)!, manifest.EntryPoint);
            manifests.Add((dllPath, manifest));
        }

        var sorted = dependencyResolver.SortByDependencies(manifests.Select(x => x.manifest));

        foreach (var manifest in sorted)
        {
            var dll = manifests.First(x => x.manifest.Id == manifest.Id).dll;
            LoadSingle(dll, manifest);
        }
    }

    public void LoadSingle(string dllPath, PluginManifest manifest)
    {
        if (_loaded.ContainsKey(dllPath))
            return;

        var asm = assemblyLoader.Load(dllPath);
        var module = activator.Create(asm, manifest);

        module.RegisterSignals(bus);
        _loaded[dllPath] = (asm, module, manifest);

        Console.WriteLine($"Loaded Plugin: {manifest.Name} v{manifest.Version}");
    }

    public void Unload(string dllPath)
    {
        if (!_loaded.TryGetValue(dllPath, out var plugin))
            return;

        plugin.module.UnregisterSignals(bus);
        _loaded.Remove(dllPath);

        Console.WriteLine($"Unloaded Plugin: {plugin.manifest.Name}");
    }

    public IReadOnlyCollection<PluginManifest> LoadedManifests
        => _loaded.Values.Select(v => v.manifest).ToArray();
}
