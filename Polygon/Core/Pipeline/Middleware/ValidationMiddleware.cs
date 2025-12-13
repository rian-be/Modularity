using Core.Features.Pipeline.Abstractions;
using Polygon.Core.Context;

namespace Polygon.Core.Pipeline.Middleware;

public class ValidationMiddleware : IMiddleware<MyContext>
{
    public async Task InvokeAsync(MyContext context, Func<Task> next)
    {
        if (context.Roles == null || context.Roles.Length == 0)
            throw new InvalidOperationException("Roles cannot be empty.");
        
        await next();
    }
}