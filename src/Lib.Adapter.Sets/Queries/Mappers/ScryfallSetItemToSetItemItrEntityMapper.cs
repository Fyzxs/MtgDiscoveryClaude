using System.Collections.Generic;
using System.Linq;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Sets.Entities;
using Lib.Shared.DataModels.Entities;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;

namespace Lib.Adapter.Sets.Queries.Mappers;

/// <summary>
/// Maps Scryfall set storage items to ITR entities within the adapter layer.
/// </summary>
internal sealed class ScryfallSetItemToSetItemItrEntityMapper
{
    public ISetItemItrEntity Map(ScryfallSetItem scryfallSetItem)
    {
        dynamic data = scryfallSetItem.Data;
        ICollection<ISetGroupingItrEntity> groupings = Groupings(data);

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
            PrintedSize = data.printed_size ?? 0,
            Groupings = groupings
        };
    }

    private static ICollection<ISetGroupingItrEntity> Groupings(dynamic data)
    {
        ICollection<ISetGroupingItrEntity> groupings = [];
        try
        {
            dynamic groupingsData = data.groupings;
            if (groupingsData != null)
            {
                string groupingsJson = JsonConvert.SerializeObject(groupingsData);
                List<SetGroupingItrEntity> groupingsList = JsonConvert.DeserializeObject<List<SetGroupingItrEntity>>(groupingsJson);

                if (groupingsList is not null)
                {
                    groupings = [.. groupingsList.Cast<ISetGroupingItrEntity>()];
                }
            }
        }
        catch (RuntimeBinderException)
        {
            // If groupings property doesn't exist, leave as empty collection
        }
        catch (JsonException)
        {
            // If parsing fails, leave as empty collection
        }

        return groupings;
    }
}
