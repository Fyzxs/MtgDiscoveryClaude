using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

public sealed class UserSetCardExtEntity : CosmosItem
{
    [JsonProperty("id")]
    public override string Id => SetId;

    [JsonProperty("partition")]
    public override string Partition => UserId;

    [JsonProperty("user_id")]
    public string UserId { get; init; }

    [JsonProperty("set_id")]
    public string SetId { get; init; }

    [JsonProperty("total_cards")]
    public int TotalCards { get; init; }

    [JsonProperty("unique_cards")]
    public int UniqueCards { get; init; }

    [JsonProperty("collecting")]
    public ICollection<string> GroupsCollecting { get; init; } = [];

    [JsonProperty("groups")]
    public Dictionary<string, UserSetCardGroupExtEntity> Groups { get; init; } = [];
}
