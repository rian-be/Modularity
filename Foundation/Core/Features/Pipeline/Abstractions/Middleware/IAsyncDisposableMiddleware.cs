namespace Core.Features.Pipeline.Abstractions.Middleware;

/// <summary>
/// Represents a middleware component that owns asynchronous resources
/// and requires explicit asynchronous disposal.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Implemented by middleware that allocates resources such as streams, locks, or external connections.</item>
/// <item>Allows the pipeline runtime to deterministically release resources after execution.</item>
/// <item>Complements <see cref="IAsyncDisposable"/> semantics without coupling middleware to runtime ownership rules.</item>
/// <item>Typically invoked by the pipeline executor during teardown or scope finalization.</item>
/// </list>
/// </remarks>
public interface IAsyncDisposableMiddleware
{
    /// <summary>
    /// Asynchronously releases resources owned by the middleware instance.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous dispose operation.
    /// </returns>
    ValueTask DisposeAsync();
}