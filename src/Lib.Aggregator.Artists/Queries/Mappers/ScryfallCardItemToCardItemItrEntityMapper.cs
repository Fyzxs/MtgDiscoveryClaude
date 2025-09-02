using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Aggregator.Artists.Models;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal sealed class ScryfallCardItemToCardItemItrEntityMapper
{
    public ICardItemItrEntity Map(ScryfallCardItem scryfallCard)
    {
        if (scryfallCard == null) return null;

        return new CardItemItrEntity(scryfallCard);
    }
}