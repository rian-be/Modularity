namespace Core.Features.Pipeline.Abstractions;

public interface IMiddleware<in TContext>
{
    Task InvokeAsync(TContext context, Func<Task> next);
}