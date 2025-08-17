using System.Threading.Tasks;

namespace Lib.Scryfall.Ingestion.Processors;

internal interface IArtistAggregateProcessor
{
    Task ProcessAsync();
}