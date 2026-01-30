using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis.Aggregation;

namespace Lib.Scryfall.Ingestion.Processors;

internal interface IArtistProcessor
{
    Task ProcessAsync(IArtistAggregate artist);
}
