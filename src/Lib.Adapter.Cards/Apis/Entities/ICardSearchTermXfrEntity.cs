namespace Lib.Adapter.Cards.Apis.Entities;

/// <summary>
/// Transfer representation of a card search term used by the adapter layer.
/// This entity crosses the Aggregator→Adapter boundary when no actual entity mapping is needed,
/// providing a simple wrapper for search term values in external system operations.
/// </summary>
public interface ICardSearchTermXfrEntity
{
    /// <summary>
    /// The search term for finding cards.
    /// Typically represents the search query as needed by the external data source.
    /// </summary>
    string SearchTerm { get; }
}
