using System.Collections.Generic;
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

    [JsonProperty("counts")]
    public FinishCountsExtEntity Counts { get; init; }

    [JsonProperty("collecting_finishes")]
    public IReadOnlyCollection<string> CollectingFinishes { get; init; } = [];
}
