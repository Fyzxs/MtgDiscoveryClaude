using System.Threading.Tasks;

namespace Lib.Scryfall.Ingestion.Apis;

public interface IScryfallIngestionService
{
    Task IngestAllSetsAsync();
}
