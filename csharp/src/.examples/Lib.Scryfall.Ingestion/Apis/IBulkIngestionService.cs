using System.Threading.Tasks;

namespace Lib.Scryfall.Ingestion.Apis;

public interface IBulkIngestionService
{
    Task IngestBulkDataAsync();
}
