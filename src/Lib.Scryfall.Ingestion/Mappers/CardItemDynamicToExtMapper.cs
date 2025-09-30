using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Scryfall.Ingestion.Services;
using Newtonsoft.Json.Linq;

namespace Lib.Scryfall.Ingestion.Mappers;

internal sealed class CardItemDynamicToExtMapper : ICardItemDynamicToExtMapper
{
    private readonly ICardGroupingMatcher _groupingMatcher;

    public CardItemDynamicToExtMapper() : this(new CardGroupingMatcher())
    {
    }

    private CardItemDynamicToExtMapper(ICardGroupingMatcher groupingMatcher)
    {
        _groupingMatcher = groupingMatcher;
    }

    public ScryfallCardItemExtEntity Map(dynamic scryfallCard)
    {
        dynamic data = scryfallCard;

        // Get the set code from the card
        string setCode = GetSetCode(data);

        // Determine the group for this card
        string groupId = _groupingMatcher.GetGroupIdForCard(data, setCode);

        // Add set_group_id to the card data if a group was found
        if (string.IsNullOrEmpty(groupId) is false)
        {
            JObject dataObject = JObject.FromObject(data);
            dataObject["set_group_id"] = groupId;
            data = dataObject;
        }

        return new ScryfallCardItemExtEntity { Data = data };
    }

    private static string GetSetCode(dynamic data)
    {
        if (data is JObject jobj)
        {
            if (jobj.TryGetValue("set", out JToken token))
            {
                return token.ToString();
            }
        }
        return null;
    }
}
