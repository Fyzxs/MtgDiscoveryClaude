using System;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Nesteds;

public sealed class RulingEntryItem
{
    [JsonProperty("source")]
    public string Source { get; init; }

    [JsonProperty("published_at")]
    public DateTime PublishedAt { get; init; }

    [JsonProperty("comment")]
    public string Comment { get; init; }
}
