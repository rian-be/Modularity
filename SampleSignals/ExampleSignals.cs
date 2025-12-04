using Signals.Interfaces;
using Signals.Loader;

namespace SampleSignals;

/// <summary>
/// A sample signal module that registers and handles <see cref="ExampleEvent"/> with an <see cref="IEventBus"/>.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Demonstrates how to implement <see cref="ISignalModule"/> to provide modular event handling.</item>
/// <item>Registers an event handler for <see cref="ExampleEvent"/> during <see cref="RegisterSignals"/>.</item>
/// <item>Unregisters the event handler during <see cref="UnregisterSignals"/> to support hot-unloading.</item>
/// <item>Handler <see cref="OnExampleEvent"/> logs the event message and Id to the console.</item>
/// <item>Serves as a template for creating additional signal modules in a modular Signals architecture.</item>
/// </list>
/// </remarks>
public class ExampleSignals : ISignalModule
{
    /// <inheritdoc />
    public void RegisterSignals(IEventBus bus)
    {
        bus.Subscribe<ExampleEvent>(OnExampleEvent);
        Console.WriteLine("ExampleSignals registered ExampleEvent");
    }
    
    /// <inheritdoc />
    public void UnregisterSignals(IEventBus bus)
    {
        bus.Unsubscribe<ExampleEvent>(OnExampleEvent);
        Console.WriteLine("ExampleSignals unregistered ExampleEvent");
    }
    
    /// <summary>
    /// Handler invoked when an <see cref="ExampleEvent"/> is published on the bus.
    /// </summary>
    /// <param name="evt">The event instance containing message and Id.</param>
    /// <returns>A completed <see cref="Task"/>.</returns>
    private Task OnExampleEvent(ExampleEvent evt)
    {
        Console.WriteLine($"ExampleSignals received event: {evt.Message} (Id: {evt.Id})");
        return Task.CompletedTask;
    }
}
