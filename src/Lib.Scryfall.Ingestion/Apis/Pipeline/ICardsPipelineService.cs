using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib.Scryfall.Ingestion.Apis.Pipeline;

public interface ICardsPipelineService
{
    Task<int> ProcessCardsAsync();
}