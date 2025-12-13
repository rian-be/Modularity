using Core.Features.Pipeline.Abstractions.Middleware;

namespace Core.Features.Pipeline.Abstractions.Abstracts;

/// <summary>
/// Base class for middleware that supports asynchronous disposal.
/// </summary>
/// <typeparam name="TContext">The type of the pipeline execution context.</typeparam>
/// <remarks>
/// <list type="bullet">
/// <item>Inherits from <see cref="MiddlewareBase{TContext}"/> providing a standard middleware foundation.</item>
/// <item>Implements <see cref="IAsyncDisposableMiddleware"/> to allow cleanup of async resources when pipeline completes.</item>
/// <item>Provides a virtual <see cref="DisposeAsyncCore"/> method for derived classes to override with custom disposal logic.</item>
/// <item>Ensures disposal occurs only once, even if <see cref="DisposeAsync"/> is called multiple times.</item>
/// </list>
/// </remarks>
public abstract class AsyncDisposableMiddlewareBase<TContext> :
    MiddlewareBase<TContext>,
    IAsyncDisposableMiddleware
{
    private bool _disposed;
    
    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        _disposed = true;

        await DisposeAsyncCore();
    }

    protected virtual ValueTask DisposeAsyncCore() =>
        ValueTask.CompletedTask;
}