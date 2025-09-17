using System.Collections.Generic;

namespace Lib.Adapter.Cards.Apis.Entities;

/// <summary>
/// Transfer representation of card identifiers used by the adapter layer.
/// This entity crosses the Aggregator→Adapter boundary when no actual entity mapping is needed,
/// providing a simple wrapper for card ID collection values in external system operations.
/// </summary>
public interface ICardIdsXfrEntity
{
    /// <summary>
    /// The collection of unique identifiers for cards.
    /// Typically represents the cards' IDs as stored in the external data source.
    /// </summary>
    IEnumerable<string> CardIds { get; }
}