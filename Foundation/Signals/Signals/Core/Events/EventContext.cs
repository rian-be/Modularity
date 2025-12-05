namespace Signals.Core.Events;

/// <summary>
/// Represents context information for an event as it flows through the pipeline.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Contains tracing and correlation identifiers for observability and debugging.</item>
/// <item>Includes W3C-compliant <see cref="TraceParent"/> and optional <see cref="TraceState"/>.</item>
/// <item>Provides <see cref="CorrelationId"/> and <see cref="RequestId"/> for correlating related events and requests.</item>
/// <item>Optionally tracks the user responsible for the event via <see cref="UserId"/>.</item>
/// <item>Includes <see cref="Timestamp"/> indicating when the context was created.</item>
/// <item>Use <see cref="Create"/> to generate a new context with automatic IDs and timestamps.</item>
/// </list>
/// </remarks>
/// <param name="TraceParent">W3C traceparent identifier for distributed tracing.</param>
/// <param name="TraceState">Optional W3C tracestate string.</param>
/// <param name="CorrelationId">Identifier to correlate events belonging to the same flow.</param>
/// <param name="RequestId">Identifier for the specific request triggering the event.</param>
/// <param name="UserId">Optional identifier of the user related to the event.</param>
/// <param name="Timestamp">Time when the context was created.</param>
public sealed record EventContext(
    string TraceParent,
    string? TraceState,
    string CorrelationId,
    string RequestId,
    string? UserId,
    DateTimeOffset Timestamp
)
{
    public static EventContext Create(
        string? userId = null,
        string? parentTraceParent = null,
        string? traceState = null)
    {
        var traceId = parentTraceParent is not null
            ? ExtractTraceId(parentTraceParent)
            : GenerateTraceId();

        var spanId = GenerateSpanId();

        var traceParent = $"00-{traceId}-{spanId}-01";

        return new EventContext(
            TraceParent: traceParent,
            TraceState: traceState,
            CorrelationId: Ulid.NewUlid().ToString(),
            RequestId: Ulid.NewUlid().ToString(),
            UserId: userId,
            Timestamp: DateTimeOffset.UtcNow
        );
    }

    private static string GenerateTraceId()
        => Guid.NewGuid().ToString("N"); // 32 hex

    private static string GenerateSpanId()
        => Guid.NewGuid().ToString("N")[..16]; // 8 bytes = 16 hex

    private static string ExtractTraceId(string traceParent)
        => traceParent.Split('-')[1];
}