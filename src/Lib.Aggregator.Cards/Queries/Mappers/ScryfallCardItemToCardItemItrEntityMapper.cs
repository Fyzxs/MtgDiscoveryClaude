using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Aggregator.Cards.Models;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal sealed class ScryfallCardItemToCardItemItrEntityMapper
{
    public ICardItemItrEntity Map(ScryfallCardItem scryfallCard)
    {
        if (scryfallCard == null) return null;

        return new CardItemItrEntity(scryfallCard);
    }
}
