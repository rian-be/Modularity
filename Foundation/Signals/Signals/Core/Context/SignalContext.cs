using Microsoft.Extensions.Logging;
using Signals.Core.Bus;
using Signals.Core.Events;

namespace Signals.Core.Context;

/// <summary>
/// Provides a contextual wrapper around <see cref="IEventBus"/> for a signal,
/// allowing convenient emission of events and sending requests with a shared <see cref="EventContext"/>.
/// </summary>
/// <param name="EventCtx">The current event context for tracing and correlation.</param>
/// <param name="Bus">The event bus instance.</param>
public sealed record SignalContext(EventContext EventCtx, IEventBus Bus, ILogger? Logger = null)
{
    /// <summary>
    /// Publishes a single event through the bus.
    /// </summary>
    /// <param name="evt">The event to publish.</param>
    public Task Emit(IEvent evt)
    {
        if (evt is IEventWithContext ctxEvt)
            ctxEvt.SetContext(EventCtx);

        Logger?.LogInformation(
            "[SignalContext] Emit: {EventType}, TraceParent={TraceParent}, CorrelationId={CorrelationId}",
            evt.GetType().Name,
            EventCtx.TraceParent,
            EventCtx.CorrelationId
        );
        return Bus.Publish(evt);
    }
    /// <summary>
    /// Publishes multiple events through the bus using the same <see cref="EventContext"/>.
    /// </summary>
    /// <param name="events">Events to publish.</param>
    public Task EmitBatch(params IEvent[] events)
    {
        foreach (var evt in events.OfType<IEventWithContext>())
            evt.SetContext(EventCtx);

        Logger?.LogInformation(
            "[SignalContext] EmitBatch: {Count} events, TraceParent={TraceParent}, CorrelationId={CorrelationId}",
            events.Length,
            EventCtx.TraceParent,
            EventCtx.CorrelationId
        );

        return Bus.PublishBatch(events);
    }
    /// <summary>
    /// Sends a request and awaits a typed response.
    /// </summary>
    /// <typeparam name="TRequest">Request type implementing <see cref="IEvent"/>.</typeparam>
    /// <typeparam name="TResponse">Expected response type implementing <see cref="IResponseEvent"/>.</typeparam>
    /// <param name="req">The request instance.</param>
    /// <returns>The response event.</returns>
    public async Task<TResponse> Request<TRequest, TResponse>(TRequest req)
        where TRequest : IEvent
        where TResponse : IResponseEvent
    {
        if (req is IEventWithContext ctxReq)
        {
            ctxReq.SetContext(EventCtx);
        }

        Logger?.LogInformation(
            "[SignalContext] Request: {RequestType}, TraceParent={TraceParent}, CorrelationId={CorrelationId}",
            typeof(TRequest).Name,
            EventCtx.TraceParent,
            EventCtx.CorrelationId
        );
        return await Bus.Send<TRequest, TResponse>(req);
    }
}