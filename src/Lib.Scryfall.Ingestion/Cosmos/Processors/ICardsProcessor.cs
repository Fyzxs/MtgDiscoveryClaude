using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis.Models;

namespace Lib.Scryfall.Ingestion.Cosmos.Processors;

internal interface ICardsProcessor
{
    Task ProcessAsync(IScryfallCard card);
}
