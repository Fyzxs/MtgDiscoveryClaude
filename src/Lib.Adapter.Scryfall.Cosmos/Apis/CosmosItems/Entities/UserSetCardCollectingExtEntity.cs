using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;

public sealed class UserSetCardCollectingExtEntity
{
    [JsonProperty("set_group_id")]
    public string SetGroupId { get; init; }

    [JsonProperty("collecting")]
    public bool Collecting { get; init; }

    [JsonProperty("count")]
    public int Count { get; init; }
}
