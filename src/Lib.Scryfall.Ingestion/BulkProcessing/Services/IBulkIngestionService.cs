using System.Threading.Tasks;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Services;

internal interface IBulkIngestionService
{
    Task IngestBulkDataAsync();
}