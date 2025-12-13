using Core.Features.Pipeline.Abstractions.Middleware;
using Polygon.Core.Context;

namespace Polygon.Core.Pipeline.Middleware.Impl;

/// <summary>
/// Middleware responsible for logging the start and end of pipeline execution for a given <see cref="MyContext"/>.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Implements <see cref="IMiddlewareDescriptor"/> to expose metadata about this middleware.</item>
/// <item>Marked as <see cref="MiddlewareKind.Diagnostic"/> for diagnostic purposes.</item>
/// <item>Non-terminal and non-conditional middleware; always executes for every context.</item>
/// <item>Includes metadata specifying logging level.</item>
/// </list>
/// </remarks>
public class LoggingMiddleware : IMiddleware<MyContext>, IMiddlewareDescriptor
{
    /// <inheritdoc />
    public string Name => "Logging";

    /// <inheritdoc />
    public MiddlewareKind Kind => MiddlewareKind.Diagnostic;

    /// <inheritdoc />
    public bool IsTerminal => false;

    /// <inheritdoc />
    public bool IsConditional => false;

    /// <inheritdoc />
    public Type MiddlewareType => GetType();

    /// <inheritdoc />
    public IReadOnlyDictionary<string, object?> Metadata =>
        new Dictionary<string, object?>
        {
            ["Level"] = "Information"
        };

    /// <inheritdoc />
    public async Task InvokeAsync(MyContext context, Func<Task> next)
    {
        Console.WriteLine($"[LoggingMiddleware]: Context: {context.Id}");
        await next();
    }
}