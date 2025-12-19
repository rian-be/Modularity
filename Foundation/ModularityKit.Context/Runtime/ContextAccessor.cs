using ModularityKit.Context.Abstractions;

namespace ModularityKit.Context.Runtime;

/// <summary>
/// Provides access to the current <typeparamref name="TContext"/> instance.
/// </summary>
/// <typeparam name="TContext">The type of context, must implement <see cref="IContext"/>.</typeparam>
/// <remarks>
/// <list type="bullet">
/// <item>Implements <see cref="IContextAccessor{TContext}"/> to expose the current context.</item>
/// <item>Delegates storage and retrieval of the context to <see cref="ContextStore{TContext}"/>.</item>
/// <item>Throws an exception if <see cref="RequireCurrent"/> is called when no context is active.</item>
/// </list>
/// </remarks>
public sealed class ContextAccessor<TContext>(ContextStore<TContext> store) : IContextAccessor<TContext>
    where TContext : class, IContext
{
    /// <inheritdoc />
    public TContext? Current => store.Current;
    
    /// <inheritdoc />
    public TContext RequireCurrent()
    {
        return Current ?? throw new InvalidOperationException(
            $"No active {typeof(TContext).Name} found. " +
            $"Ensure the context is set before accessing it.");
    }
}