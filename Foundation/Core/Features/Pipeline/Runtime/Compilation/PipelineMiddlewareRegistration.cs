using Core.Features.Pipeline.Abstractions.Middleware;

namespace Core.Features.Pipeline.Runtime.Compilation;

/// <summary>
/// Represents a precompiled middleware registration, containing a factory delegate for execution
/// and a metadata descriptor for inspection.
/// </summary>
/// <typeparam name="TContext">The type of context that the middleware operates on.</typeparam>
/// <remarks>
/// <list type="bullet">
/// <item>Encapsulates the compiled delegate returned by <see cref="ICompilableMiddleware{TContext}.Compile"/>.</item>
/// <item>Holds an <see cref="IMiddlewareDescriptor"/> that provides metadata about the middleware.</item>
/// <item>Used by <see cref="IPipelineCompiler{TContext}"/> to build a <see cref="PipelineCompilation{TContext}"/>.</item>
/// <item>Enables high-performance pipeline execution by pre-composing middleware delegates.</item>
/// </list>
/// </remarks>
public sealed class PipelineMiddlewareRegistration<TContext>(
    Func<Func<TContext, ValueTask>, Func<TContext, ValueTask>> factory,
    IMiddlewareDescriptor descriptor)
{
    /// <summary>
    /// Gets the compiled middleware factory delegate.
    /// </summary>
    public Func<Func<TContext, ValueTask>, Func<TContext, ValueTask>> Factory { get; } = factory;
  
    /// <summary>
    /// Gets the metadata descriptor for this middleware.
    /// </summary>
    public IMiddlewareDescriptor Descriptor { get; } = descriptor;
}