using Signals.Core.Events;

namespace SampleSignals.Model.Ping;

public sealed record PongEvent(string Message) : IResponseEvent;