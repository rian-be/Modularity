using Core.Features.Pipeline.Runtime.Compilation;

namespace Core.Features.Pipeline.Abstractions.Middleware;

/// <summary>
/// Represents a middleware that can be precompiled into a <see cref="PipelineMiddlewareRegistration{TContext}"/> 
/// for high-performance pipeline execution.
/// </summary>
/// <typeparam name="TContext">The type of context that the middleware operates on.</typeparam>
/// <remarks>
/// <list type="bullet">
/// <item>Provides a <see cref="Compile"/> method that transforms the middleware into a registration object.</item>
/// <item>Compiled registrations are consumed by <see cref="IPipelineCompiler{TContext}"/> to produce a <see cref="PipelineCompilation{TContext}"/>.</item>
/// <item>Enables separation of middleware declaration and execution logic, improving runtime performance.</item>
/// </list>
/// </remarks>
public interface ICompilableMiddleware<TContext>
{
    /// <summary>
    /// Compiles the middleware into a <see cref="PipelineMiddlewareRegistration{TContext}"/> containing
    /// a pre-composed delegate and metadata descriptor.
    /// </summary>
    /// <returns>A <see cref="PipelineMiddlewareRegistration{TContext}"/> representing the compiled middleware.</returns>
    PipelineMiddlewareRegistration<TContext> Compile();
}