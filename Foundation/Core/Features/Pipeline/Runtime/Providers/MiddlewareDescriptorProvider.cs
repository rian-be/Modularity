using System.Collections.Concurrent;
using Core.Features.Pipeline.Abstractions;
using Core.Features.Pipeline.Abstractions.Middleware;
using Core.Features.Pipeline.Abstractions.Middleware.Attributes;

namespace Core.Features.Pipeline.Runtime.Providers;

public class MiddlewareDescriptorProvider
{
    private readonly ConcurrentDictionary<Type, IMiddlewareDescriptor> _cache = new();

    public IMiddlewareDescriptor GetDescriptor(object middleware)
    {
        var type = middleware.GetType();
        return _cache.GetOrAdd(type, t =>
        {
            if (typeof(IMiddlewareDescriptor).IsAssignableFrom(t))
            {
                var instance = Activator.CreateInstance(t)!;
                return (IMiddlewareDescriptor)instance;
            }
            
            var attr = t.GetCustomAttributes(typeof(MiddlewareDescriptorAttribute), false)
                .Cast<MiddlewareDescriptorAttribute>()
                .FirstOrDefault();

            var metadataAttrs = t.GetCustomAttributes(typeof(MiddlewareMetadataAttribute), false)
                .Cast<MiddlewareMetadataAttribute>();

            var metadata = metadataAttrs.ToDictionary(a => a.Key, a => a.Value);

            if (attr != null)
            {
                metadata["Name"] = attr.Name;
                metadata["Kind"] = attr.Kind;
                metadata["IsTerminal"] = attr.IsTerminal;
                metadata["IsConditional"] = attr.IsConditional;

                foreach (var kv in attr.Metadata)
                    metadata[kv.Key] = kv.Value;
            }

            if (t.GetCustomAttributes(typeof(ConditionalMiddlewareAttribute), false).Length != 0)
            {
                metadata["IsConditional"] = true;
            }

            return new MiddlewareDescriptor(
                type,
                attr?.Name ?? type.Name,
                attr?.Kind ?? MiddlewareKind.Standard,
                attr?.IsTerminal ?? false,
                metadata.ContainsKey("IsConditional") && (bool)metadata["IsConditional"]!,
                metadata
            );
        });
    }
}