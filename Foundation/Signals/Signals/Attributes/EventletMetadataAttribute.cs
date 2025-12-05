namespace Signals.Attributes;

/// <summary>
/// Provides metadata for an Eventlet.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Defines <see cref="Name"/>, <see cref="Version"/>, <see cref="Author"/> and <see cref="Description"/>.</item>
/// <item>Used by the Eventlet loader to identify Eventlets and build documentation or diagnostics.</item>
/// <item>Does not affect Eventlet behavior directly; jest tylko do celów opisowych i discovery.</item>
/// </list>
/// </remarks>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class EventletMetadataAttribute(string name, string version, string author, string description)
    : Attribute
{
    public string Name { get; } = name;
    public string Version { get; } = version;
    public string Author { get; } = author;
    public string Description { get; } = description;
}