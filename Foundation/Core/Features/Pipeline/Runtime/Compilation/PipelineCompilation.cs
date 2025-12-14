using Core.Features.Pipeline.Abstractions.Middleware;

namespace Core.Features.Pipeline.Runtime.Compilation;

/// <summary>
/// Represents a compiled, immutable middleware pipeline for a specific <typeparamref name="TContext"/> type.
/// </summary>
/// <typeparam name="TContext">The type of context the pipeline operates on.</typeparam>
/// <remarks>
/// <list type="bullet">
/// <item>Holds the composed <see cref="EntryPoint"/> delegate that executes all middleware in order.</item>
/// <item>Contains descriptors for each middleware to allow inspection or diagnostics.</item>
/// <item>Used by <see cref="IPipelineCompiler{TContext}"/> and pipeline executors for high-performance execution.</item>
/// <item>Immutable after creation to ensure thread-safety and predictable execution behavior.</item>
/// </list>
/// </remarks>
public sealed class PipelineCompilation<TContext>(
    Func<TContext, ValueTask> entryPoint,
    IMiddlewareDescriptor[] descriptors)
{
    public Func<TContext, ValueTask> EntryPoint { get; } = entryPoint;

    public IReadOnlyList<IMiddlewareDescriptor> Descriptors { get; } = descriptors;
}