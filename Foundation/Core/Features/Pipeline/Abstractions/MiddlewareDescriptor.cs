using Core.Features.Pipeline.Abstractions.Middleware;

namespace Core.Features.Pipeline.Abstractions;

public sealed record MiddlewareDescriptor(
    Type MiddlewareType,
    string Name,
    MiddlewareKind Kind,
    bool IsTerminal,
    bool IsConditional,
    IReadOnlyDictionary<string, object?> Metadata
) : IMiddlewareDescriptor;