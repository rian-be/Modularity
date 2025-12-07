using SampleSignals.Model;
using SampleSignals.Model.Ping;
using Signals.Attributes;
using Signals.Core.Context;
using Signals.Core.Events;

namespace SampleSignals.Handler;

[HandlesRequest(typeof(PingEvent), typeof(PongEvent))]
public class PingHandler : IRequestHandler<PingEvent, PongEvent>
{
    public async Task<PongEvent> Handle(PingEvent evt, SignalContext ctx)
    {
        await ctx.Emit(new ExampleEvent($"ExampleEvent received: {evt.Message}"));
        return new PongEvent($"Pong to: {evt.Message}");
    }
}