using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib.Scryfall.Ingestion.Apis.Pipeline;

public interface ICardsPipelineService
{
    Task<IReadOnlyList<dynamic>> FetchCardsAsync();
    void ProcessCards(IReadOnlyList<dynamic> cards, IReadOnlyDictionary<string, dynamic> setsByCode);
    Task WriteCardsAsync(IReadOnlyList<dynamic> cards);
}