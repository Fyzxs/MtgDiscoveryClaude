namespace Lib.Adapter.Artists.Apis.Entities;

/// <summary>
/// Transfer representation of an artist identifier used by the adapter layer.
/// This entity crosses the Aggregator→Adapter boundary when no actual entity mapping is needed,
/// providing a simple wrapper for artist ID values in external system operations.
/// </summary>
public interface IArtistIdXfrEntity
{
    /// <summary>
    /// The unique identifier for the artist.
    /// Typically represents the artist's ID as stored in the external data source.
    /// </summary>
    string ArtistId { get; }
}