using System.Reflection;

namespace Signals.Runtime.Interfaces;

/// <summary>
/// Defines a service responsible for loading plugin assemblies from DLL files.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Abstracts the assembly loading mechanism, allowing customization or mocking for testing.</item>
/// <item>Supports dynamic plugin systems where assemblies are loaded at runtime.</item>
/// <item>Decouples assembly loading from module instantiation and plugin management.</item>
/// </list>
/// </remarks>
public interface IModuleAssemblyLoader
{
    /// <summary>
    /// Loads an assembly from the specified DLL path.
    /// </summary>
    /// <param name="dllPath">The full path to the plugin DLL.</param>
    /// <returns>The loaded <see cref="Assembly"/> instance.</returns>
    Assembly Load(string dllPath);
}