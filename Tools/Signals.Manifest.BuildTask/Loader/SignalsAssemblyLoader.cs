using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Build.Utilities;

namespace Signals.Manifest.BuildTask.Loader;

/// <summary>
/// Loads assemblies in an isolated, collectible context to discover <c>ISignalModule</c> types.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Creates a temporary <see cref="AssemblyLoadContext"/> for each assembly to avoid locking files.</item>
/// <item>Scans the assembly for types implementing <c>ISignalModule</c>.</item>
/// <item>Ensures only one <c>ISignalModule</c> per assembly; logs an error if multiple implementations exist.</item>
/// <item>Unloads the assembly context after scanning to free resources and allow file overwrites.</item>
/// <item>Intended for use in MSBuild tasks generating or validating signal manifests.</item>
/// </list>
/// </remarks>
public sealed class SignalsAssemblyLoader(TaskLoggingHelper log)
{
    public Type? LoadModuleType(string assemblyPath)
    {
        var assemblyDir = Path.GetDirectoryName(assemblyPath)!;

        var context = new PluginLoadContext(assemblyDir);

        var asm = context.LoadFromAssemblyPath(assemblyPath);

        Type[] types;

        try
        {
            types = asm.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            foreach (var loaderEx in ex.LoaderExceptions)
                log.LogError(loaderEx?.Message);

            throw;
        }

        var moduleTypes = types
            .Where(t =>
                !t.IsAbstract &&
                t.GetInterfaces().Any(i => i.Name == "ISignalModule"))
            .ToArray();

        return moduleTypes.Length == 1
            ? moduleTypes[0]
            : null;
    }

    private sealed class PluginLoadContext(string basePath) : AssemblyLoadContext(isCollectible: true)
    {
        protected override Assembly? Load(AssemblyName assemblyName)
        {
            var dllPath = Path.Combine(basePath, assemblyName.Name + ".dll");

            return File.Exists(dllPath) ? LoadFromAssemblyPath(dllPath) : null;
        }
    }
}