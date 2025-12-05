using System.Reflection;
using Signals.Runtime.Interfaces;

namespace Signals.Runtime.Loader;

/// <inheritdoc />
public sealed class DefaultAssemblyLoader : IModuleAssemblyLoader
{
    /// <inheritdoc />
    public Assembly Load(string dllPath) =>
         !File.Exists(dllPath) ? throw new FileNotFoundException($"DLL not found: {dllPath}") : Assembly.LoadFrom(dllPath);
}