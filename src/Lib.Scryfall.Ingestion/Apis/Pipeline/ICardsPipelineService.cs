using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Apis.Pipeline;

public interface ICardsPipelineService
{
    Task<IReadOnlyList<IScryfallCard>> FetchCardsAsync();
    void ProcessCards(IReadOnlyList<IScryfallCard> cards, IReadOnlyDictionary<string, dynamic> setsByCode);
    Task WriteCardsAsync(IReadOnlyList<IScryfallCard> cards);
}