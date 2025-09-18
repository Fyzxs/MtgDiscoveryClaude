using System;
using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;

public sealed class RulingEntryExtEntity : CosmosEntity
{
    [JsonProperty("source")]
    public string Source { get; init; }

    [JsonProperty("published_at")]
    public DateTime PublishedAt { get; init; }

    [JsonProperty("comment")]
    public string Comment { get; init; }
}
