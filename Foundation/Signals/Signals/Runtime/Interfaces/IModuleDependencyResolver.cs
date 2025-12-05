using Signals.Runtime.Manifest;

namespace Signals.Runtime.Interfaces;

/// <summary>
/// Responsible for resolving and sorting plugin manifests based on declared dependencies.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Ensures plugins are loaded in the correct order, respecting their dependency relationships.</item>
/// <item>Prevents runtime errors caused by missing dependencies during plugin activation.</item>
/// <item>Can be used by plugin loaders or managers to determine a safe load sequence.</item>
/// <item>Supports complex dependency graphs and can throw exceptions if circular dependencies are detected.</item>
/// </list>
/// </remarks>
public interface IModuleDependencyResolver
{
    /// <summary>
    /// Sorts a collection of <see cref="ModuleManifest"/> instances according to their dependencies.
    /// </summary>
    /// <param name="manifests">The plugin manifests to sort.</param>
    /// <returns>An array of <see cref="ModuleManifest"/> sorted in dependency order.</returns>
    ModuleManifest[] SortByDependencies(IEnumerable<ModuleManifest> manifests);
}