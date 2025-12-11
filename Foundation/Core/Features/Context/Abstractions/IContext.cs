namespace Core.Features.Context.Abstractions;

/// <summary>
/// Represents generic context with unique identifier and creation timestamp.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Provides a unique <see cref="Id"/> to identify the context instance.</item>
/// <item>Tracks the creation time of the context via <see cref="CreatedAt"/>.</item>
/// <item>Can be implemented by various context types to support ambient or scoped data.</item>
/// </list>
/// </remarks>
public interface IContext
{
    /// <summary>
    /// Gets the unique identifier of this context instance.
    /// </summary>
    string Id { get; }
    
    /// <summary>
    /// Gets the timestamp when this context was created.
    /// </summary>
    DateTimeOffset CreatedAt { get; }
}