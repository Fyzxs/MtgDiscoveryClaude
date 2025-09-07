using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Aggregator.Scryfall.Shared.Entities;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Scryfall.Shared.Mappers;

public sealed class ScryfallCardItemToCardItemItrEntityMapper
{
    public ICardItemItrEntity Map(ScryfallCardItem scryfallCard) => new CardItemItrEntity(scryfallCard);
}
