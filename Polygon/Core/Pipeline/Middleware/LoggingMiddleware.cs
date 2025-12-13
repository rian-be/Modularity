using Core.Features.Pipeline.Abstractions;
using Polygon.Core.Context;

namespace Polygon.Core.Pipeline.Middleware;

public class LoggingMiddleware : IMiddleware<MyContext>
{
    public async Task InvokeAsync(MyContext context, Func<Task> next)
    {
        Console.WriteLine($"Start processing: {context.Id}");
        await next();
        Console.WriteLine($"End processing: {context.Id}");
    }
}