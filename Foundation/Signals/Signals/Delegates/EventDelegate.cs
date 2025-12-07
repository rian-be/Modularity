using Signals.Core.Bus;
using Signals.Core.Context;
using Signals.Core.Events;
using Signals.Core.Handlers;

namespace Signals.Delegates;

/// <summary>
/// Represents a handler for an <see cref="IEvent"/> that executes asynchronously.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Used internally by <see cref="IEventBus"/> and Eventlets to process published events.</item>
/// <item>Supports asynchronous execution of event handling logic.</item>
/// <item>Receives an <see cref="EventContext"/> to provide metadata, tracing, or control flags for the event.</item>
/// <item>Can be wrapped in <see cref="HandlerWrapper"/> to provide priority, filtering, and once-only semantics.</item>
/// </list>
/// </remarks>
/// <param name="evt">The event instance being handled.</param>
/// <param name="ctx">Context of the event, providing metadata or pipeline control.</param>
/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
public delegate Task EventDelegate(IEvent evt, EventContext ctx);