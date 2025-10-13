using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.Cards.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class CollectionStringToCardNameSearchResultMapper : ICollectionStringToCardNameSearchResultMapper
{
    public Task<ICollection<ICardNameSearchResultItrEntity>> Map(IEnumerable<string> cardNames)
    {
        List<ICardNameSearchResultItrEntity> results = [];

        foreach (string name in cardNames)
        {
            results.Add(new CardNameSearchResultItrEntity { Name = name });
        }

        return Task.FromResult<ICollection<ICardNameSearchResultItrEntity>>(results);
    }
}
