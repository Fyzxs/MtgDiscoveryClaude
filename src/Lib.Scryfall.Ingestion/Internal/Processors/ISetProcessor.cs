using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis.Models;

namespace Lib.Scryfall.Ingestion.Internal.Processors;
internal interface ISetProcessor
{
    Task ProcessAsync(IScryfallSet set);
}
