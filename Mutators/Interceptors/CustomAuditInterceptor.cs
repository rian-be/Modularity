using ModularityKit.Mutators.Abstractions.Changes;
using ModularityKit.Mutators.Abstractions.Context;
using ModularityKit.Mutators.Abstractions.Intent;
using ModularityKit.Mutators.Abstractions.Policies;
using ModularityKit.Mutators.Runtime.Interception;

namespace Mutators.Interceptors;

public class CustomAuditInterceptor : MutationInterceptorBase
{
    public override string Name => "CustomAudit";
    public override int Order => 10;

    protected override bool ShouldRun(MutationIntent intent, MutationContext context)
    {
        var isSecurityCategory = intent.Category == "Security";
        var hasAuthTag = intent.Tags.Contains("auth");

        return isSecurityCategory && hasAuthTag;
    }

    public override Task OnBeforeMutationAsync(MutationIntent intent, MutationContext context, object state, string executionId, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"[Audit-Before] {intent.OperationName} by {context.ActorName}");
        return Task.CompletedTask;
    }

    public override Task OnAfterMutationAsync(MutationIntent intent, MutationContext context, object? oldState, object? newState, ChangeSet changes, string executionId, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"[Audit-After] {intent.OperationName} - {changes.Changes.Count} changes applied");
        return Task.CompletedTask;
    }

    public override Task OnMutationFailedAsync(MutationIntent intent, MutationContext context, object state, Exception exception, string executionId, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"[Audit-Failed] {intent.OperationName} failed: {exception.Message}");
        return Task.CompletedTask;
    }

    public override Task OnPolicyBlockedAsync(MutationIntent intent, MutationContext context, object state, PolicyDecision decision, string executionId, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"[Audit-Blocked] {intent.OperationName} blocked: {decision.Reason}");
        return Task.CompletedTask;
    }
}
