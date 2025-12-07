using Signals.Core.Events;

namespace SampleSignals.Model.Ping;

public sealed record PingEvent(string Message) : IEvent;