using Signals.Core.Bus;
using Signals.Runtime.Loader;

namespace Signals.Core.Modules;

/// <summary>
/// Represents a modular plugin that can register and unregister signals (events and handlers) with an <see cref="IEventBus"/>.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Each module encapsulates a set of related events and handlers.</item>
/// <item>Modules can be loaded dynamically via <see cref="SignalsLoader"/>.</item>
/// <item>Provides explicit methods to register and unregister signals to support hot-loading and unloading of plugins.</item>
/// <item>Allows the event system to remain modular, extensible, and maintainable.</item>
/// </list>
/// </remarks>
public interface ISignalModule
{
    /// <summary>
    /// Registers the module's signals (events and handlers) with the provided <see cref="IEventBus"/>.
    /// </summary>
    /// <param name="bus">The event bus to register signals with.</param>
    void RegisterSignals(IEventBus bus);

    /// <summary>
    /// Unregisters the module's signals from the provided <see cref="IEventBus"/>.
    /// </summary>
    /// <param name="bus">The event bus to unregister signals from.</param>
    void UnregisterSignals(IEventBus bus);
}