using Core.Features.Pipeline.Abstractions;

namespace Core.Features.Pipeline.Runtime;

/// <summary>
/// Executes a configured middleware pipeline for a given <typeparamref name="TContext"/> instance.
/// </summary>
/// <typeparam name="TContext">The type of context the pipeline operates on.</typeparam>
/// <remarks>
/// <list type="bullet">
/// <item>Consumes a <see cref="PipelineBuilder{TContext}"/> and executes its middlewares in order.</item>
/// <item>Supports asynchronous middleware execution with a continuation delegate (<c>Next</c>).</item>
/// <item>Implements <see cref="IPipelineExecutor{TContext}"/> to provide a standard pipeline execution interface.</item>
/// <item>Each middleware can decide whether to invoke the next middleware in the chain.</item>
/// </list>
/// </remarks>
public class PipelineExecutor<TContext>(PipelineBuilder<TContext> builder) : IPipelineExecutor<TContext>
{
    private readonly Func<TContext, Func<Task>, Task>[] _middlewares = builder.Middlewares.ToArray();
    
    /// <inheritdoc />
    public async Task ExecuteAsync(TContext context)
    {
        var index = -1;

        await Next();
        return;

        Task Next()
        {
            index++;
            if (index < _middlewares.Length)
                return _middlewares[index](context, Next);

            return Task.CompletedTask;
        }
    }
}