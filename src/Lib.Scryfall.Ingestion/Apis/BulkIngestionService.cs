using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Apis;

/// <summary>
/// Public API for bulk data ingestion.
/// </summary>
public sealed class BulkIngestionService : IBulkIngestionService
{
    private readonly BulkProcessing.Services.IBulkIngestionService _internalService;

    public BulkIngestionService(ILogger logger)
    {
        _internalService = new BulkProcessing.Services.BulkIngestionService(logger);
    }

    public async Task IngestBulkDataAsync()
    {
        await _internalService.IngestBulkDataAsync().ConfigureAwait(false);
    }
}