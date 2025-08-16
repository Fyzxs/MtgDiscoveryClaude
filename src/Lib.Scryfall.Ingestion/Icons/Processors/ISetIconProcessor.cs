using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis.Models;

namespace Lib.Scryfall.Ingestion.Icons.Processors;

public interface ISetIconProcessor
{
    Task ProcessAsync(IScryfallSet set);
}