using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Signals.Manifest.BuildTask.Logging;

/// <summary>
/// Provides centralized logging utilities for generated signal manifests during MSBuild execution.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Logs a standardized warning block after a signal manifest is generated.</item>
/// <item>Highlights fields that require mandatory manual verification.</item>
/// <item>Uses <see cref="TaskLoggingHelper"/> to integrate with the MSBuild logging pipeline.</item>
/// <item>Does not perform any file system or validation logic.</item>
/// </list>
/// </remarks>
internal static class SignalManifestLogHelper
{ 
    /// <summary>
    /// Logs a structured warning message indicating that a signal manifest was generated automatically.
    /// </summary>
    /// <param name="buildEngine">The MSBuild engine used for logging.</param>
    /// <param name="taskItem">Optional task item that triggered the generation.</param>
    /// <param name="outputFile">Full path to the generated signal manifest file.</param>
    public static void LogGeneratedManifest(IBuildEngine buildEngine, ITaskItem? taskItem, string outputFile)
    {
        var logger = new TaskLoggingHelper(buildEngine, nameof(SignalManifestLogHelper));

        logger.LogWarning($"""
                           ============================================================
                            SIGNAL MANIFEST GENERATED AUTOMATICALLY
                           ============================================================

                           File:
                            {outputFile}

                           ⚠ MANUAL REVIEW REQUIRED
                           ------------------------------------------------------------
                           Please verify the following fields before publishing:

                            • Id                – Must be globally unique and stable
                            • Name              – Human-readable display name
                            • Description       – Clear description of purpose
                            • Author            – Organization or developer name
                            • Version           – Semantic version
                            • Dependencies      – Required Signals / Modules
                            • EntryPoint        – Main module entry type

                           ✅ This file was generated automatically.
                           ✅ You are responsible for validating its correctness.

                           ============================================================
                           """);
    }
}