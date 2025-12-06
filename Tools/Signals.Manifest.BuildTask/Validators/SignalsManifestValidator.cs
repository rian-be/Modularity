using Microsoft.Build.Utilities;
using Signals.Manifest.BuildTask.Models;

namespace Signals.Manifest.BuildTask.Validators;

/// <summary>
/// Validates <see cref="SignalsManifest"/> instances for correctness before publishing.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Ensures semantic correctness of <see cref="SignalsManifest.Version"/> and <see cref="SignalsManifest.ApiVersion"/> using <see cref="SemverValidator"/>.</item>
/// <item>Verifies mandatory fields such as <c>Id</c> and <c>EntryPoint</c> are not empty.</item>
/// <item>Logs all errors through the MSBuild <see cref="TaskLoggingHelper"/> pipeline.</item>
/// <item>Does not throw exceptions; returns a boolean indicating overall validity.</item>
/// </list>
/// </remarks>
public static class SignalsManifestValidator
{
    /// <summary>
    /// Validates the specified signal manifest.
    /// </summary>
    /// <param name="manifest">The signal manifest to validate.</param>
    /// <param name="log">The MSBuild <see cref="TaskLoggingHelper"/> used to report errors.</param>
    /// <returns><c>true</c> if the manifest passes all checks; otherwise, <c>false</c>.</returns>
    public static bool Validate(SignalsManifest manifest, TaskLoggingHelper log)
    {
        var valid = SemverValidator.Validate(manifest.Version, log);
        
        if (!SemverValidator.Validate(manifest.ApiVersion, log))
            valid = false;
        
        if (string.IsNullOrWhiteSpace(manifest.Id))
        {
            log.LogError("The Id manifest cannot be empty.");
            valid = false;
        }

        if (!string.IsNullOrWhiteSpace(manifest.EntryPoint)) return valid;
        log.LogError("MThe EntryPoint manifest cannot be empty.");
        valid = false;

        return valid;
    }
}