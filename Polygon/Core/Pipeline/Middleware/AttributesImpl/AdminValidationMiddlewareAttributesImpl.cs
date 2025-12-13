using Core.Features.Pipeline.Abstractions.Middleware;
using Core.Features.Pipeline.Abstractions.Middleware.Attributes;
using Polygon.Core.Context;

namespace Polygon.Core.Pipeline.Middleware.AttributesImpl;

/// <summary>
/// Middleware that validates if the current <see cref="MyContext"/> has the "Admin" role.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Applied with <see cref="ConditionalMiddlewareAttribute"/> to indicate it only runs under certain conditions.</item>
/// <item>If the user does not have the "Admin" role, execution is skipped and the next middleware in the pipeline is invoked.</item>
/// <item>Intended for scenarios where certain pipeline steps should be restricted to administrative users.</item>
/// </list>
/// </remarks>
[ConditionalMiddleware(Description = "Only runs for Admins")]
public class AdminValidationMiddlewareAttributesImpl : IMiddleware<MyContext>
{
    /// <summary>
    /// Executes the middleware logic.
    /// </summary>
    /// <param name="context">The current <see cref="MyContext"/> instance.</param>
    /// <param name="next">The delegate to invoke the next middleware in the pipeline.</param>
    public async Task InvokeAsync(MyContext context, Func<Task> next)
    {
        if (!context.Roles.Contains("Admin"))
        {
            Console.WriteLine("Skipping AdminValidationMiddleware because user is not Admin");
            await next();
            return;
        }

        Console.WriteLine("Running AdminValidationMiddleware");
        await next();
    }
}