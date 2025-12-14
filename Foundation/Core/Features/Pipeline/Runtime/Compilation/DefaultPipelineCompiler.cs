using Core.Features.Pipeline.Abstractions.Middleware;

namespace Core.Features.Pipeline.Runtime.Compilation;

/// <summary>
/// Default implementation of <see cref="IPipelineCompiler{TContext}"/> that compiles
/// a list of middleware registrations into a single executable pipeline delegate.
/// </summary>
/// <typeparam name="TContext">The type of context the pipeline operates on.</typeparam>
public sealed class DefaultPipelineCompiler<TContext> : IPipelineCompiler<TContext>
{
    private static readonly Func<TContext, ValueTask> Completed = static _ => ValueTask.CompletedTask;

    /// <summary>
    /// Compiles a list of <see cref="PipelineMiddlewareRegistration{TContext}"/> into a single
    /// <see cref="PipelineCompilation{TContext}"/> containing the composed delegate and descriptors.
    /// </summary>
    /// <param name="registrations">The ordered list of middleware registrations.</param>
    /// <returns>
    /// A <see cref="PipelineCompilation{TContext}"/> with the compiled pipeline delegate
    /// and corresponding middleware descriptors.
    /// </returns>
    public PipelineCompilation<TContext> Compile(IReadOnlyList<PipelineMiddlewareRegistration<TContext>> registrations)
    {
        if (registrations.Count == 0)
            return new PipelineCompilation<TContext>(Completed, Array.Empty<IMiddlewareDescriptor>());

        var next = Completed;
        for (var i = registrations.Count - 1; i >= 0; i--)
        {
            next = registrations[i].Factory(next);
        }

        var descriptors = registrations.Select(r => r.Descriptor).ToArray();
        return new PipelineCompilation<TContext>(next, descriptors);
    }
}