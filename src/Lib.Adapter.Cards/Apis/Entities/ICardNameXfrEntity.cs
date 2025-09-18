namespace Lib.Adapter.Cards.Apis.Entities;

/// <summary>
/// Transfer representation of a card name used by the adapter layer.
/// This entity crosses the Aggregator→Adapter boundary when no actual entity mapping is needed,
/// providing a simple wrapper for card name values in external system operations.
/// </summary>
public interface ICardNameXfrEntity
{
    /// <summary>
    /// The name of the card.
    /// Typically represents the card's name as stored in the external data source.
    /// </summary>
    string CardName { get; }
}
