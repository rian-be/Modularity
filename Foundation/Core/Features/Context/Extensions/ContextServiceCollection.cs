using Core.Features.Context.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Features.Context.Extensions;

/// <summary>
/// Provides extension methods to register context-related services into the DI container.
/// </summary>
public static class ContextServiceCollection
{
    /// <summary>
    /// Registers services required for managing a <typeparamref name="TContext"/> in dependency injection.
    /// </summary>
    /// <typeparam name="TContext">The type of context, must implement <see cref="IContext"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which context services will be added.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance for chaining.</returns>
    /// <remarks>
    /// <list type="bullet">
    /// <item>Registers <see cref="ContextStore{TContext}"/> as a singleton for storing current context.</item>
    /// <item>Registers <see cref="IContextAccessor{TContext}"/> as a singleton to access the current context.</item>
    /// <item>Registers <see cref="IContextManager{TContext}"/> as a singleton to execute code within a context.</item>
    /// </list>
    /// </remarks>
    public static IServiceCollection AddContext<TContext>(
        this IServiceCollection services)
        where TContext : class, IContext
    {
        services.AddSingleton<ContextStore<TContext>>();
        services.AddSingleton<IContextAccessor<TContext>, ContextAccessor<TContext>>();
        services.AddSingleton<IContextManager<TContext>, ContextManager<TContext>>();
        
        return services;
    }
}