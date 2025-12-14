using Core.Features.Pipeline.Abstractions.Middleware;

namespace Core.Features.Pipeline.Runtime.Compilation;

/// <summary>
/// Defines a compiler that transforms a collection of <see cref="PipelineMiddlewareRegistration{TContext}"/> 
/// into a high-performance, compiled <see cref="PipelineCompilation{TContext}"/>.
/// </summary>
/// <typeparam name="TContext">The type of context that the pipeline operates on.</typeparam>
/// <remarks>
/// <list type="bullet">
/// <item>Consumes middleware registrations created by <see cref="ICompilableMiddleware{TContext}"/> components.</item>
/// <item>Produces an immutable <see cref="PipelineCompilation{TContext}"/> with a pre-composed entry point delegate.</item>
/// <item>Designed to improve runtime performance by reducing middleware dispatch overhead.</item>
/// <item>Implementations can be swapped or customized to modify compilation behavior.</item>
/// </list>
/// </remarks>
public interface IPipelineCompiler<TContext>
{
    /// <summary>
    /// Compiles the given middleware registrations into a <see cref="PipelineCompilation{TContext}"/>.
    /// </summary>
    /// <param name="registrations">The ordered list of middleware registrations to compile.</param>
    /// <returns>
    /// A <see cref="PipelineCompilation{TContext}"/> instance containing the compiled entry point
    /// and metadata descriptors for all middlewares.
    /// </returns>
    PipelineCompilation<TContext> Compile(
        IReadOnlyList<PipelineMiddlewareRegistration<TContext>> registrations);
}