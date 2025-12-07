using SampleSignals.Model.Ping;
using Signals.Attributes;
using Signals.Core.Events;

namespace SampleSignals.Handler;

[HandlesRequest(typeof(PingEvent), typeof(PongEvent))]
public class PingHandler : IRequestHandler<PingEvent, PongEvent>
{
    public Task<PongEvent> Handle(PingEvent evt)
    {
        Console.WriteLine($"Event received: {evt.Message} and reply was sent back");
        return Task.FromResult(new PongEvent($"Pong to: {evt.Message}"));
    }
}