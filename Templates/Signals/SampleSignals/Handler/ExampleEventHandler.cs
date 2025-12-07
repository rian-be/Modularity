using SampleSignals.Model;
using Signals.Attributes;
using Signals.Core.Events;

namespace SampleSignals.Handler;

[HandlesEvent(typeof(ExampleEvent))]
public class ExampleEventHandler : IEventHandler<ExampleEvent>
{
    public Task Handle(ExampleEvent evt)
    {
        Console.WriteLine($"[ExampleEventHandler]: received event: {evt.Message} (Id: {evt.Id})");
        return Task.CompletedTask;
    }
}