using Core.Features.Pipeline.Abstractions.Middleware;

namespace Core.Features.Pipeline.Abstractions;

/// <summary>
/// Defines a builder interface for configuring a middleware pipeline for a specific context type.
/// </summary>
/// <typeparam name="TContext">The type of context the pipeline operates on.</typeparam>
/// <remarks>
/// <list type="bullet">
/// <item>Allows registering, ordering, and conditionally applying middleware components.</item>
/// <item>Supports inserting middleware at the beginning, before, or after specific existing components.</item>
/// <item>Supports conditional execution and branching based on context.</item>
/// <item>Enables flexible pipeline construction for processing requests, events, or commands.</item>
/// </list>
/// </remarks>
public interface IPipelineBuilder<TContext>
{
    /// <summary>
    /// Adds a middleware to the end of the pipeline.
    /// </summary>
    /// <param name="middleware">The middleware to add.</param>
    void Use(IMiddleware<TContext> middleware);

    /// <summary>
    /// Adds a middleware to the beginning of the pipeline.
    /// </summary>
    /// <param name="middleware">The middleware to add.</param>
    void UseFirst(IMiddleware<TContext> middleware);

    /// <summary>
    /// Inserts a middleware immediately after the first middleware matching the given predicate.
    /// </summary>
    /// <param name="predicate">Predicate to locate the reference middleware.</param>
    /// <param name="middleware">Middleware to insert.</param>
    void UseAfter(Predicate<IMiddleware<TContext>> predicate, IMiddleware<TContext> middleware);

    /// <summary>
    /// Inserts a middleware immediately before the first middleware matching the given predicate.
    /// </summary>
    /// <param name="predicate">Predicate to locate the reference middleware.</param>
    /// <param name="middleware">Middleware to insert.</param>
    void UseBefore(Predicate<IMiddleware<TContext>> predicate, IMiddleware<TContext> middleware);

    /// <summary>
    /// Adds a middleware that executes only when the specified condition on the context evaluates to true.
    /// </summary>
    /// <param name="condition">Predicate to evaluate the context for conditional execution.</param>
    /// <param name="middleware">Middleware to execute conditionally.</param>
    void UseWhen(Func<TContext, bool> condition, IMiddleware<TContext> middleware);

    /// <summary>
    /// Creates a conditional branch in the pipeline that executes a separate pipeline configuration
    /// when the given condition evaluates to true.
    /// </summary>
    /// <param name="condition">Predicate that determines if the branch should be executed.</param>
    /// <param name="configurePipeline">Action to configure the branch pipeline.</param>
    void UseBranch(
        Func<TContext, bool> condition,
        Action<IPipelineBuilder<TContext>> configurePipeline);
}
