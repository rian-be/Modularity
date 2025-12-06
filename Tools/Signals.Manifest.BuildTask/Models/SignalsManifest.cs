namespace Signals.Manifest.BuildTask.Models;

/// <summary>
/// Represents the metadata of a signal plugin used during build tasks.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Contains all necessary information to generate a plugin manifest JSON.</item>
/// <item>Used internally in MSBuild tasks for signals to track plugin ID, version, entry point, and dependencies.</item>
/// <item>Includes optional metadata such as author, description, and API version.</item>
/// <item>Dependencies are represented as an array of plugin IDs.</item>
/// </list>
/// </remarks>
public sealed class SignalsManifest
{
    public required string Id { get; init; }
    public required string Name { get; set; }
    public required string Version { get; init; }
    public required string EntryPoint { get; init; }
    public required string Dll { get; set; }

    public string? Author { get; set; }
    public string? Description { get; set; }
    public required string ApiVersion { get; init; }

    public string[] Dependencies { get; set; } = [];
}