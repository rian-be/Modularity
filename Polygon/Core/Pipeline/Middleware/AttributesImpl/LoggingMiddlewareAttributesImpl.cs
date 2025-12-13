using Core.Features.Pipeline.Abstractions.Middleware;
using Core.Features.Pipeline.Abstractions.Middleware.Attributes;
using Polygon.Core.Context;

namespace Polygon.Core.Pipeline.Middleware.AttributesImpl;

[MiddlewareDescriptor(Name = "Logging", Kind = MiddlewareKind.Diagnostic)]
[MiddlewareMetadata("Level", "Information")]
public class LoggingMiddlewareAttributesImpl : IMiddleware<MyContext>
{
    public async Task InvokeAsync(MyContext context, Func<Task> next)
    {
        Console.WriteLine($"[LoggingMiddlewareAttributesImpl]: Context: {context.Id}");
        await next();
    }
}