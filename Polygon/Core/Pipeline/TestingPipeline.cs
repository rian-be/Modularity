using Core.Features.Pipeline.Middleware;
using Core.Features.Pipeline.Runtime;
using Polygon.Core.Context;
using Polygon.Core.Pipeline.Middleware;

namespace Polygon.Core.Pipeline;

public class TestingPipeline
{
    public async Task RunDemo()
    {
        // -----------------------
        //  Build main pipeline
        // -----------------------
        var builder = new PipelineBuilder<MyContext>();

        Console.WriteLine("Adding LoggingMiddleware to main pipeline...");
        builder.Use(new LoggingMiddleware());

        Console.WriteLine("Adding ValidationMiddleware conditionally for Admins...");
        builder.UseWhen(ctx => ctx.Roles.Contains("Admin"), new ValidationMiddleware());

        // -----------------------
        //  Build branch pipeline
        // -----------------------
        var branchBuilder = new PipelineBuilder<MyContext>();
        Console.WriteLine("Adding LoggingMiddleware to branch pipeline...");
        branchBuilder.Use(new LoggingMiddleware());

        Console.WriteLine("Adding ValidationMiddleware conditionally for Developers in branch...");
        branchBuilder.UseWhen(ctx => ctx.Roles.Contains("Developer"), new ValidationMiddleware());

        Console.WriteLine("Adding BranchMiddleware to main pipeline for TenantId 'tenant-456'...");
        builder.Use(new BranchMiddleware<MyContext>(
            condition: ctx => ctx.TenantId == "tenant-456",
            branch: branchBuilder
        ));

        // -----------------------
        //  Create executor
        // -----------------------
        var executor = new PipelineExecutor<MyContext>(builder);

        // -----------------------
        //  Attach inspector
        // -----------------------
        var inspector = new PipelineInspector<MyContext>(builder.Middlewares);

        Console.WriteLine();
        Console.WriteLine("=== Registered middleware in main pipeline ===");
        foreach (var mw in inspector.GetMiddlewares())
        {
            Console.WriteLine($"- {mw.GetType().Name}");
        }

        Console.WriteLine();
        Console.WriteLine("Removing ValidationMiddleware dynamically...");
        inspector.Remove(mw => mw is ValidationMiddleware);

        Console.WriteLine("=== Middleware after removal ===");
        foreach (var mw in inspector.GetMiddlewares())
        {
            Console.WriteLine($"- {mw.GetType().Name}");
        }

        // -----------------------
        //  Create context
        // -----------------------
        var context = MyContext.Create(
            userId: "user-123",
            tenantId: "tenant-456",
            roles: ["Admin", "Developer"],
            userEmail: "alice@company.com"
        );

        context.Metadata["DemoStep"] = "PipelineDebug";

        // -----------------------
        //  Execute pipeline
        // -----------------------
        Console.WriteLine();
        Console.WriteLine("=== Executing pipeline ===");
        await executor.ExecuteAsync(context);
        Console.WriteLine("=== Pipeline execution complete ===");

        // -----------------------
        //  Post-execution debug
        // -----------------------
        Console.WriteLine();
        Console.WriteLine("Context metadata after execution:");
        foreach (var kvp in context.Metadata)
        {
            Console.WriteLine($"- {kvp.Key}: {kvp.Value}");
        }

        Console.WriteLine();
        Console.WriteLine("Enabled features:");
        foreach (var f in context.EnabledFeatures)
        {
            Console.WriteLine($"- {f}");
        }

        await Task.CompletedTask;
    }
}
