using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Aggregator.Sets.Models;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Sets.Queries.Mappers;

internal sealed class ScryfallSetItemToSetItemItrEntityMapper
{
    public ISetItemItrEntity Map(ScryfallSetItem scryfallSetItem)
    {
        // Use Scryfall data if available, fallback to Data for backward compatibility
        dynamic data = scryfallSetItem.Scryfall ?? scryfallSetItem.Data ?? scryfallSetItem.MtgJson ?? scryfallSetItem.MtgDiscovery;

        return new SetItemItrEntity
        {
            Id = data.id,
            Code = data.code,
            TcgPlayerId = data.tcgplayer_id ?? 0,
            Name = data.name,
            Uri = data.uri,
            ScryfallUri = data.scryfall_uri,
            SearchUri = data.search_uri,
            ReleasedAt = data.released_at,
            SetType = data.set_type,
            CardCount = data.card_count,
            Digital = data.digital,
            NonFoilOnly = data.nonfoil_only,
            FoilOnly = data.foil_only,
            BlockCode = data.block_code,
            Block = data.block,
            IconSvgUri = data.icon_svg_uri,
            PrintedSize = data.printed_size ?? 0
        };
    }
}
