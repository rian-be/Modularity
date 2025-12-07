using Signals.Core.Events;

namespace Signals.Core.Handlers;

/// <summary>
/// Thread-safe container for a single request/response handler of a specific request type.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Stores exactly one <see cref="IRequestHandler{TRequest,TResponse}"/> instance per request type.</item>
/// <item>Thread-safe via an internal lock to allow concurrent read/write operations.</item>
/// <item>Supports generic request/response types and safe casting when retrieving handlers.</item>
/// <item>Used by <see cref="SubscriptionManager"/> to manage request/response handlers.</item>
/// </list>
/// </remarks>
public sealed class RequestHandlerCollection
{
    private readonly Lock _handlerLock = new();
    private object? _handler;

    /// <summary>
    /// Sets the request handler for a specific request/response type.
    /// </summary>
    /// <typeparam name="TRequest">Type of the request event.</typeparam>
    /// <typeparam name="TResponse">Type of the response event.</typeparam>
    /// <param name="handler">Handler instance to store.</param>
    public void SetHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
        where TRequest : IEvent
        where TResponse : IResponseEvent
    {
        lock (_handlerLock)
        {
            _handler = handler;
        }
    }

    /// <summary>
    /// Gets the request handler for a specific request/response type.
    /// </summary>
    /// <typeparam name="TRequest">Type of the request event.</typeparam>
    /// <typeparam name="TResponse">Type of the response event.</typeparam>
    /// <returns>The stored <see cref="IRequestHandler{TRequest,TResponse}"/> if it exists; otherwise <c>null</c>.</returns>
    public IRequestHandler<TRequest, TResponse>? GetHandler<TRequest, TResponse>()
        where TRequest : IEvent
        where TResponse : IResponseEvent
    {
        lock (_handlerLock)
        {
            return _handler as IRequestHandler<TRequest, TResponse>;
        }
    }
}
