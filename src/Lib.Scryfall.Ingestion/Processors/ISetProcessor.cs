using System.Threading.Tasks;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Processors;

internal interface ISetProcessor
{
    Task ProcessAsync(IScryfallSet set);
}
