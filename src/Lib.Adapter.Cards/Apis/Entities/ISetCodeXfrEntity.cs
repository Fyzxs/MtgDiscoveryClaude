namespace Lib.Adapter.Cards.Apis.Entities;

/// <summary>
/// Transfer representation of a set code used by the adapter layer.
/// This entity crosses the Aggregator→Adapter boundary when no actual entity mapping is needed,
/// providing a simple wrapper for set code values in external system operations.
/// </summary>
public interface ISetCodeXfrEntity
{
    /// <summary>
    /// The code that uniquely identifies a card set.
    /// Typically represents the set's code as stored in the external data source.
    /// </summary>
    string SetCode { get; }
}