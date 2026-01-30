using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class CollectionCardNameSearchOufToOutMapper : ICollectionCardNameSearchOufToOutMapper
{
    public Task<List<CardNameSearchResultOutEntity>> Map(ICardNameSearchCollectionOufEntity search)
    {
        List<CardNameSearchResultOutEntity> result = [.. search.Names.Select(nameResult => new CardNameSearchResultOutEntity { Name = nameResult.Name })];

        return Task.FromResult(result);
    }
}
