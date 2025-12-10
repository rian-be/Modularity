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
/// <param name="Logger">Optional logger.</param>
public sealed record SignalContext(
    EventContext EventCtx,
    IEventBus Bus,
    ILogger? Logger = null)
{
    public Task<TResponse> EmitSingle<TEvent, TResponse>(TEvent evt)
        where TEvent : IEvent
        where TResponse : IResponseEvent
    {
        AttachContext(evt);

        return Bus.Send<TEvent, TResponse>(evt);
    }

    public Task<TResponse> EmitResponse<TResponse>(IEvent evt)
        where TResponse : IResponseEvent
    {
        AttachContext(evt);

        Logger?.LogInformation(
            "[SignalContext] EmitResponse: {EventType}, TraceParent={TraceParent}, CorrelationId={CorrelationId}",
            evt.GetType().Name,
            EventCtx.TraceParent,
            EventCtx.CorrelationId
        );

        return Bus.Send<IEvent, TResponse>(evt);
    }

    public async Task EmitSingleRaw(IEvent evt)
    {
        AttachContext(evt);

        Logger?.LogInformation(
            "[SignalContext] EmitSingleRaw: {EventType}",
            evt.GetType().Name
        );

        if (Bus is not EventBus busImpl)
            throw new InvalidOperationException(
                "EmitSingleRaw requires concrete EventBus");

        var wrapper = busImpl.SubscriptionManager
            .GetFirstHandler(evt.GetType());

        if (wrapper is null)
            throw new InvalidOperationException(
                $"No handler registered for {evt.GetType().Name}"
            );

        await wrapper.Handler(evt);
    }

    private void AttachContext(IEvent evt)
    {
        if (evt is IEventWithContext ctxEvt)
            ctxEvt.SetContext(EventCtx);
    }
}
