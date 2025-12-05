using Signals.Manifest;

namespace Signals.Interfaces.Loader;

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
public interface IPluginDependencyResolver
{
    /// <summary>
    /// Sorts a collection of <see cref="PluginManifest"/> instances according to their dependencies.
    /// </summary>
    /// <param name="manifests">The plugin manifests to sort.</param>
    /// <returns>An array of <see cref="PluginManifest"/> sorted in dependency order.</returns>
    PluginManifest[] SortByDependencies(IEnumerable<PluginManifest> manifests);
}