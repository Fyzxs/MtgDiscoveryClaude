namespace Lib.Scryfall.Ingestion.Apis.Models;

/// <summary>
/// Represents a Scryfall card.
/// </summary>
public interface IScryfallCard
{
    /// <summary>
    /// Gets the card name.
    /// </summary>
    string Name();

    /// <summary>
    /// Gets the raw data.
    /// </summary>
    dynamic Data();
}
