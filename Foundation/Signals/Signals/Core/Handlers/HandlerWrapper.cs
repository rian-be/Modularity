using Signals.Core.Bus;
using Signals.Core.Events;

namespace Signals.Core.Handlers;

/// <summary>
/// Wraps an event handler with additional metadata for execution.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Encapsulates a handler function that processes an <see cref="IEvent"/> asynchronously.</item>
/// <item>Supports <see cref="Priority"/> to control execution order relative to other handlers.</item>
/// <item>Optionally uses <see cref="Filter"/> to conditionally execute the handler based on the event instance.</item>
/// <item>Supports one-time handlers with <see cref="Once"/>; handler is removed after first invocation.</item>
/// <item>Used internally by <see cref="IEventBus"/> implementations to manage event dispatching.</item>
/// </list>
/// </remarks>
/// <param name="Handler">Asynchronous function to handle the event.</param>
/// <param name="Priority">Execution priority; higher values run earlier.</param>
/// <param name="Filter">Optional predicate to filter which events are handled.</param>
/// <param name="Once">Indicates if the handler should be invoked only once.</param>
public readonly record struct HandlerWrapper(
    Func<IEvent, Task> Handler,
    int Priority = 0,
    Func<IEvent, bool>? Filter = null,
    bool Once = false
);