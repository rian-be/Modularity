using System.Reflection;
using Signals.Interfaces.Loader;

namespace Signals.Loader;

/// <inheritdoc />
public sealed class DefaultAssemblyLoader : IPluginAssemblyLoader
{
    /// <inheritdoc />
    public Assembly Load(string dllPath) =>
         !File.Exists(dllPath) ? throw new FileNotFoundException($"DLL not found: {dllPath}") : Assembly.LoadFrom(dllPath);
}