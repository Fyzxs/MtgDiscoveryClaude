using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis.Models;

namespace Lib.Scryfall.Ingestion.Cosmos.Processors;

public interface ISetItemsProcessor
{
    Task ProcessAsync(IScryfallSet set);
}
