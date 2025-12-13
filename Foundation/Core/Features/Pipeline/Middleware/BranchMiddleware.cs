using Core.Features.Pipeline.Abstractions;
using Core.Features.Pipeline.Runtime;

namespace Core.Features.Pipeline.Middleware;

/// <summary>
/// Middleware that conditionally executes a branch pipeline based on a predicate.
/// </summary>
/// <typeparam name="TContext">The type of context the pipeline operates on.</typeparam>
/// <remarks>
/// <list type="bullet">
/// <item>Evaluates the provided <see cref="Func{TContext, Boolean}"/> predicate to determine if the branch should execute.</item>
/// <item>If the condition is true, executes the provided <see cref="PipelineBuilder{TContext}"/> as a separate branch pipeline.</item>
/// <item>After branch execution (or if the condition is false), calls the next middleware in the main pipeline.</item>
/// <item>Useful for conditional logic, feature flags, or scenario-specific pipeline execution.</item>
/// </list>
/// </remarks>
public class BranchMiddleware<TContext>(Func<TContext, bool> condition, PipelineBuilder<TContext> branch)
    : IMiddleware<TContext>
{
    private readonly Func<TContext, bool> _condition = condition ?? throw new ArgumentNullException(nameof(condition));
    private readonly PipelineBuilder<TContext> _branch = branch ?? throw new ArgumentNullException(nameof(branch));

    /// <inheritdoc />
    public async Task InvokeAsync(TContext context, Func<Task> next)
    {
        if (_condition(context))
        {
            var executor = new PipelineExecutor<TContext>(_branch);
            await executor.ExecuteAsync(context);
        }

        await next();
    }
}