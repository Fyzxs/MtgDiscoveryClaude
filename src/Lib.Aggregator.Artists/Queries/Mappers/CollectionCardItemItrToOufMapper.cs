using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Aggregator.Scryfall.Shared.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal sealed class CollectionCardItemItrToOufMapper : ICollectionCardItemItrToOufMapper
{
    public Task<ICardItemCollectionOufEntity> Map(IEnumerable<ICardItemItrEntity> cards)
    {
        ICollection<ICardItemItrEntity> cardCollection = [.. cards];
        ICardItemCollectionOufEntity result = new CardItemCollectionOufEntity { Data = cardCollection };

        return Task.FromResult(result);
    }
}
