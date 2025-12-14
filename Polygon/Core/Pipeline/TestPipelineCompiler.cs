using Core.Features.Pipeline.Abstractions.Middleware;
using Core.Features.Pipeline.Runtime.Compilation;
using Polygon.Core.Context;

namespace Polygon.Core.Pipeline;

/// <summary>
/// Test implementation of <see cref="IPipelineCompiler{TContext}"/> for <see cref="MyContext"/>.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Composes the pipeline from a list of <see cref="PipelineMiddlewareRegistration{TContext}"/>.</item>
/// <item>Executes registrations in reverse order to build a nested invocation chain.</item>
/// <item>Produces a <see cref="PipelineCompilation{TContext}"/> containing the entry point and middleware descriptors.</item>
/// <item>Used for testing or simple runtime compilation scenarios.</item>
/// </list>
/// </remarks>
public class TestPipelineCompiler : IPipelineCompiler<MyContext>
{    
    /// <inheritdoc />
    public PipelineCompilation<MyContext> Compile(IReadOnlyList<PipelineMiddlewareRegistration<MyContext>> registrations)
    {
        Func<MyContext, ValueTask> entryPoint = _ => ValueTask.CompletedTask;
        for (int i = registrations.Count - 1; i >= 0; i--)
        {
            var reg = registrations[i];
            entryPoint = reg.Factory(entryPoint);
        }
        var descriptors = new IMiddlewareDescriptor[registrations.Count];
        for (int i = 0; i < registrations.Count; i++)
        {
            descriptors[i] = registrations[i].Descriptor;
        }

        return new PipelineCompilation<MyContext>(entryPoint, descriptors);
    }
}