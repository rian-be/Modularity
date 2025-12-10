namespace Signals.Core.Subscriptions;

/// <summary>
/// Represents a token returned when subscribing to an event, used to manage the subscription.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Contains the <see cref="EventType"/> the subscription is associated with.</item>
/// <item>Contains a unique <see cref="Id"/> identifying the subscription.</item>
/// <item>Can be used to unsubscribe from events using the token.</item>
/// <item>Immutable and lightweight value type suitable for storage and comparison.</item>
/// </list>
/// </remarks>
/// <param name="EventType">Type of the event the subscription is for.</param>
/// <param name="Id">Unique identifier for the subscription.</param>
public readonly record struct SubscriptionToken(Type EventType, long Id);