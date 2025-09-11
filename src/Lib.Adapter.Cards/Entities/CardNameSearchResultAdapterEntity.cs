namespace Lib.Adapter.Cards.Entities;

/// <summary>
/// Adapter-specific search result entity.
/// 
/// This entity represents search results at the adapter layer and does not implement
/// ITR interfaces, maintaining proper layer separation. The aggregator layer is
/// responsible for mapping this to ICardNameSearchResultItrEntity.
/// 
/// Design Decision: Public entity for interface return type
/// While this is adapter-specific, it must be public because it's returned by
/// the public ICardQueryAdapter interface. Only aggregator layer should use this.
/// </summary>
public sealed class CardNameSearchResultAdapterEntity
{
    public string Name { get; init; } = string.Empty;
}