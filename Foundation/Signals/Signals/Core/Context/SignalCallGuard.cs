namespace Signals.Core.Context;

/// <summary>
/// Guards against excessive or recursive signal dispatches to prevent stack overflows and infinite loops.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Tracks the current call stack of signal types using <see cref="AsyncLocal{T}"/> for async-safety.</item>
/// <item>Enforces a maximum dispatch depth (<c>MaxDepth</c>) to avoid runaway recursion.</item>
/// <item>Detects recursive dispatch of the same signal type and throws an <see cref="InvalidOperationException"/> with the dispatch chain.</item>
/// <item>Provides a disposable scope via <see cref="EnterScope{TSignal}"/> to automatically manage entering and exiting signal contexts.</item>
/// </list>
/// </remarks>
public sealed class SignalCallGuard
{
    private const int MaxDepth = 50;
    private static readonly AsyncLocal<Stack<Type>> CurrentStack = new();

    private Stack<Type> Stack => CurrentStack.Value ??= new Stack<Type>();

    /// <summary>
    /// Enters a guarded scope for the specified signal type.
    /// </summary>
    /// <typeparam name="TSignal">Type of the signal being dispatched.</typeparam>
    /// <returns>A disposable scope that exits the signal context on disposal.</returns>
    public IDisposable EnterScope<TSignal>() => EnterScope(typeof(TSignal));

    private IDisposable EnterScope(Type type)
    {
        Enter(type);
        return new Scope(this);
    }

    private void Enter(Type type)
    {
        if (Stack.Count >= MaxDepth)
            throw new InvalidOperationException(
                $"Signal dispatch depth exceeded {MaxDepth}. Possible infinite loop.");

        if (Stack.Contains(type))
        {
            var chain = string.Join(" → ", Stack.Reverse().Select(t => t.Name));
            throw new InvalidOperationException(
                $"Recursive signal dispatch: {chain} → {type.Name}");
        }

        Stack.Push(type);
    }

    private void Exit()
    {
        if (Stack.Count == 0)
            throw new InvalidOperationException(
                "SignalCallGuard.Exit called without matching Enter");
        
        Stack.Pop();
    }

    private sealed class Scope(SignalCallGuard guard) : IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            guard.Exit();
        }
    }
}
