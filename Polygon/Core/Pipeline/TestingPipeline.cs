using Core.Features.Pipeline.Diagnostics;
using Core.Features.Pipeline.Runtime;
using Polygon.Core.Context;
using Polygon.Core.Pipeline.Middleware.AttributesImpl;
using Polygon.Core.Pipeline.Middleware.Impl;

namespace Polygon.Core.Pipeline;

public class TestingPipeline
{
    private readonly PipelineBuilder<MyContext> _builder;
  
    public TestingPipeline()
    {
        _builder = new PipelineBuilder<MyContext>();
        ConfigureMainPipeline(_builder);
    }
    
    private void ConfigureMainPipeline(PipelineBuilder<MyContext> builder)
    {
        builder.Use(new LoggingMiddleware());
        builder.UseWhen(ctx => ctx.Roles.Contains("User"), new ValidationMiddleware());

        var branchBuilder = new PipelineBuilder<MyContext>();
        branchBuilder.Use(new LoggingMiddleware());
        branchBuilder.Use(new LoggingMiddlewareAttributesImpl());
        branchBuilder.UseWhen(ctx => ctx.Roles.Contains("Developer"), new ValidationMiddleware());

        builder.UseBranch(
            condition: ctx => ctx.TenantId == "tenant-456",
            configurePipeline: b =>
            {
                foreach (var mw in branchBuilder.Middlewares)
                    b.Use(mw);
            }
        );
    }
    
    public async Task RunDemo()
    {
        using var scope = PipelineDebugScope.Begin(out var debug);

        var executor = new PipelineExecutor<MyContext>(_builder, true);

        var context = MyContext.Create(
            userId: "user-123",
            tenantId: "tenant-456",
            roles: ["Admin", "User"],
            userEmail: "alice@company.com"
        );
        context.Metadata["DemoStep"] = "PipelineDebug";

        await executor.ExecuteAsync(context);

        DisplayDebugInfo(context, debug);
        DisplayInspectorInfo();
    }

    private void DisplayDebugInfo(MyContext context, PipelineDebugContext debug)
    {
        Console.WriteLine();
        Console.WriteLine("=== Pipeline execution complete ===");
        Console.WriteLine("Context metadata after execution:");
        foreach (var kvp in context.Metadata)
            Console.WriteLine($"- {kvp.Key}: {kvp.Value}");

        Console.WriteLine("Enabled features:");
        foreach (var f in context.EnabledFeatures)
            Console.WriteLine($"- {f}");

        Console.WriteLine();
        Console.WriteLine("Pipeline debug steps:");
        foreach (var step in debug.Steps)
        {
            if (step.NextCalled)
            {
                Console.WriteLine($"{step.Middleware} | Duration={step.Duration?.TotalMilliseconds:F3}ms | NextCalled={step.NextCalled}");
            }
        }
    }
    
    private void DisplayInspectorInfo()
    {
        var inspector = new PipelineInspector<MyContext>(_builder.Middlewares);

        Console.WriteLine("=== Registered middleware ===");
        foreach (var mw in inspector.GetMiddlewares())
            Console.WriteLine($"- {mw.GetType().Name}");

        Console.WriteLine("Pipeline middleware descriptors:");
        foreach (var d in inspector.GetDescriptors())
            Console.WriteLine($"- {d.Name} [{d.Kind}] Conditional={d.IsConditional}");
    }
}
