using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.Cards;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class CollectionCardNameSearchOufToOutMapper : ICollectionCardNameSearchOufToOutMapper
{
    public CollectionCardNameSearchOufToOutMapper()
    {
    }

    public async Task<List<CardNameSearchResultOutEntity>> Map(ICardNameSearchCollectionOufEntity search)
    {
        await Task.CompletedTask.ConfigureAwait(false);

        return [.. search.Names.Select(nameResult => new CardNameSearchResultOutEntity { Name = nameResult.Name })];
    }
}
