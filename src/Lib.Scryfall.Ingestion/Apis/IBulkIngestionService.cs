using System.Threading.Tasks;

namespace Lib.Scryfall.Ingestion.Apis;

/// <summary>
/// Service for ingesting Scryfall bulk data.
/// </summary>
public interface IBulkIngestionService
{
    /// <summary>
    /// Ingests bulk data from Scryfall including sets, cards, artists, and rulings.
    /// </summary>
    Task IngestBulkDataAsync();
}