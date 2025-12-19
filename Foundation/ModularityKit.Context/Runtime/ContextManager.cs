using ModularityKit.Context.Abstractions;

namespace ModularityKit.Context.Runtime;

/// <summary>
/// Manages the execution of code within a specific <typeparamref name="TContext"/> scope.
/// </summary>
/// <typeparam name="TContext">The type of context, must implement <see cref="IContext"/>.</typeparam>
/// <remarks>
/// <list type="bullet">
/// <item>Implements <see cref="IContextManager{TContext}"/> to provide context-scoped execution.</item>
/// <item>Exposes the current active context via <see cref="Current"/>.</item>
/// <item>Ensures context is properly set and restored using <see cref="ContextStore{TContext}"/>.</item>
/// <item>Supports asynchronous execution with <see cref="ExecuteInContext"/> overloads for tasks returning <c>void</c> or results.</item>
/// </list>
/// </remarks>
public sealed class ContextManager<TContext>(ContextStore<TContext> store) : IContextManager<TContext>
    where TContext : class, IContext
{
    /// <inheritdoc />
    public TContext? Current => store.Current;

    /// <inheritdoc />
    public async Task ExecuteInContext(TContext context, Func<Task> action)
    {
        using (store.SetCurrent(context))
        {
            await action();
        }
    }
    
    /// <inheritdoc />
    public async Task<TResult> ExecuteInContext<TResult>(TContext context, Func<Task<TResult>> func)
    {
        using (store.SetCurrent(context))
        {
            return await func();
        }
    }
}