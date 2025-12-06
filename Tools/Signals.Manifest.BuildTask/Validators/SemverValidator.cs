using System.Text.RegularExpressions;
using Microsoft.Build.Utilities;

namespace Signals.Manifest.BuildTask.Validators;

/// <summary>
/// Provides utilities to validate semantic version strings (MAJOR.MINOR.PATCH) for signal manifests.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Used during build tasks to ensure <c>SignalsManifest.Version</c> conforms to semantic versioning.</item>
/// <item>Logs errors to MSBuild if the version is missing or invalid.</item>
/// <item>Supports optional pre-release and build metadata according to semver 2.0.0.</item>
/// </list>
/// </remarks>
public static class SemverValidator
{
    private static readonly Regex SemverRegex = new(
        @"^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)(?:-[0-9A-Za-z\-\.]+)?(?:\+[0-9A-Za-z\-\.]+)?$",
        RegexOptions.Compiled);
    
    /// <summary>
    /// Validates that the given version string matches semantic versioning.
    /// </summary>
    /// <param name="version">The version string to validate (e.g., "1.0.0").</param>
    /// <param name="log">The MSBuild <see cref="TaskLoggingHelper"/> used to report errors.</param>
    /// <returns><c>true</c> if the version is valid; otherwise, <c>false</c> and an error is logged.</returns>
    public static bool Validate(string version, TaskLoggingHelper log)
    {
        if (string.IsNullOrWhiteSpace(version))
        {
            log.LogError("Version cannot be empty.");
            return false;
        }

        if (SemverRegex.IsMatch(version)) return true;
        log.LogError($"Version '{version}' is not a valid semantic version (MAJOR.MINOR.PATCH).");
        return false;
    }
}