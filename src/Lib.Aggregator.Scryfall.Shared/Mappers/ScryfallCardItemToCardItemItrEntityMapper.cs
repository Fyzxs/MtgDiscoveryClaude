using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Aggregator.Scryfall.Shared.Models;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Scryfall.Shared.Mappers;

public sealed class ScryfallCardItemToCardItemItrEntityMapper
{
    public ICardItemItrEntity Map(ScryfallCardItem scryfallCard)
    {
        if (scryfallCard == null) return null;

        return new CardItemItrEntity(scryfallCard);
    }
}