using Signals.Attributes;
using Signals.Core.Bus;

namespace Signals.ApiBase.Registrar;

public sealed class RequestHandlerRegistrar : IHandlerRegistrar
{
    public bool TryRegister(Type handlerType, IEventBus bus, IRequestHandlerRegistry? registry)
    {
        if (registry == null) return false;

        var requestAttr = handlerType.GetCustomAttributes(typeof(HandlesRequestAttribute), false)
            .Cast<HandlesRequestAttribute>()
            .FirstOrDefault();
        if (requestAttr == null) return false;

        var handlerInstance = Activator.CreateInstance(handlerType) 
                              ?? throw new InvalidOperationException($"Cannot create instance of {handlerType.FullName}");
        
        registry.RegisterHandler((dynamic)handlerInstance, bus);

        return true;
    }
}