using ModularityKit.Context.Abstractions;

namespace ModularityKit.Context.Runtime;

/// <summary>
/// Thread-safe and async-safe storage for <typeparamref name="TContext"/> instances using <see cref="AsyncLocal{T}"/>.
/// </summary>
/// <typeparam name="TContext">The type of context to store, must implement <see cref="IContext"/>.</typeparam>
/// <remarks>
/// <list type="bullet">
/// <item>Provides access to the currently active context via <see cref="Current"/>.</item>
/// <item>Ensures that context flows correctly in asynchronous and multithreaded code.</item>
/// <item>Supports setting a new context with automatic restoration of the previous context through <see cref="SetCurrent"/>.</item>
/// <item>Provides <see cref="Clear"/> to remove any active context.</item>
/// </list>
/// </remarks>
public sealed class ContextStore<TContext> where TContext : class, IContext
{
    private static readonly AsyncLocal<TContext?> CurrentContext = new();

    /// <summary>
    /// Gets the currently active context, or <c>null</c> if no context is set.
    /// </summary>
    public TContext? Current
    {
        get => CurrentContext.Value;
        private set => CurrentContext.Value = value;
    }

    /// <summary>
    /// Sets the current context and returns a disposable scope that restores the previous context upon disposal.
    /// </summary>
    /// <param name="context">The context instance to set as current.</param>
    /// <returns>An <see cref="IDisposable"/> which restores the previous context when disposed.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is <c>null</c>.</exception>
    public IDisposable SetCurrent(TContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        var previous = Current;
        Current = context;
        
        return new ContextScope(this, previous);
    }
    
    public void Clear() => Current = null;

    private sealed class ContextScope(ContextStore<TContext> store, TContext? previousContext) : IDisposable
    {
        private int _disposed;

        /// <summary>
        /// Restores the previous context when disposed.
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, 1) == 0)
                store.Current = previousContext;
        }
    }
}
