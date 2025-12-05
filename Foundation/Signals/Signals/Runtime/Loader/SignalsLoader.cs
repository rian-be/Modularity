using System.Reflection;
using Signals.Core.Bus;
using Signals.Core.Modules;
using Signals.Runtime.Interfaces;
using Signals.Runtime.Manifest;

namespace Signals.Runtime.Loader;

/// <summary>
/// Responsible for loading, activating, and unloading signal plugins from assemblies.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Uses <see cref="IModuleManifestReader"/> to read plugin manifests from JSON files.</item>
/// <item>Resolves dependencies between plugins using <see cref="IModuleDependencyResolver"/>.</item>
/// <item>Loads assemblies via <see cref="IModuleAssemblyLoader"/> and activates signal modules via <see cref="IModuleActivator"/>.</item>
/// <item>Registers and unregisters signals in the provided <see cref="IEventBus"/>.</item>
/// <item>Maintains an internal collection of loaded plugins to prevent duplicate loading.</item>
/// <item>Supports loading multiple plugins from a directory and respects dependency order.</item>
/// </list>
/// </remarks>
public sealed class SignalsLoader(
    IEventBus bus,
    IModuleManifestReader manifestReader,
    IModuleAssemblyLoader assemblyLoader,
    IModuleDependencyResolver dependencyResolver,
    IModuleActivator activator)
{
    private readonly Dictionary<string, (Assembly asm, ISignalModule module, ModuleManifest manifest)> _loaded = new();

    public void LoadFromDirectory(string directory)
    {
        var manifests = new List<(string dll, ModuleManifest manifest)>();

        foreach (var manifestPath in Directory.GetFiles(directory, "signal.plugin.json", SearchOption.AllDirectories))
        {
            var manifest = manifestReader.Read(manifestPath);
            
            var dllFileName = string.IsNullOrWhiteSpace(manifest.Dll) ? manifest.Id + ".dll" : manifest.Dll;
            var dllPath = Path.Combine(Path.GetDirectoryName(manifestPath)!, dllFileName);

            if (!File.Exists(dllPath))
                throw new FileNotFoundException($"DLL for plugin '{manifest.Id}' not found at {dllPath}");

            manifests.Add((dllPath, manifest));
        }
        var sorted = dependencyResolver.SortByDependencies(manifests.Select(x => x.manifest));

        foreach (var manifest in sorted)
        {
            var dll = manifests.First(x => x.manifest.Id == manifest.Id).dll;
            LoadSingle(dll, manifest);
        }
    }

    public void LoadSingle(string dllPath, ModuleManifest manifest)
    {
        if (_loaded.ContainsKey(dllPath))
            return;

        var asm = assemblyLoader.Load(dllPath);
        
        var type = asm.GetType(manifest.EntryPoint, throwOnError: true);
        if (type == null)
            throw new TypeLoadException($"Type '{manifest.EntryPoint}' not found in assembly '{dllPath}'.");

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

    public IReadOnlyCollection<ModuleManifest> LoadedManifests
        => _loaded.Values.Select(v => v.manifest).ToArray();
}
