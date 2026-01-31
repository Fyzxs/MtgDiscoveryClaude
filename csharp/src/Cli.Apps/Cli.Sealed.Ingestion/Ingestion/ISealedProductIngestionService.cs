using System.Threading;
using System.Threading.Tasks;
using Cli.Sealed.Ingestion.Dtos;

namespace Cli.Sealed.Ingestion.Ingestion;

internal interface ISealedProductIngestionService
{
    Task<bool> IngestProductAsync(
        string setCode,
        string setName,
        MtgJsonSealedProductDto product,
        CancellationToken cancellationToken);
}
