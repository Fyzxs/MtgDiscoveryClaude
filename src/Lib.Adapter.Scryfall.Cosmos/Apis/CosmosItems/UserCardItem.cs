using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Nesteds;
using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

public sealed class UserCardItem : CosmosItem
{
    public override string Id => CardId;
    public override string Partition => UserId;

    [JsonProperty("user_id")]
    public string UserId { get; init; }

    [JsonProperty("card_id")]
    public string CardId { get; init; }

    [JsonProperty("set_id")]
    public string SetId { get; init; }

    [JsonProperty("collected")]
    public IEnumerable<CollectedItem> CollectedList { get; init; }
}
