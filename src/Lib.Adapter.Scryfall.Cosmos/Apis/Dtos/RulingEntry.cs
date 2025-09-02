using System;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Dtos;

public sealed class RulingEntry
{
    [JsonProperty("source")]
    public string Source { get; init; } = string.Empty;

    [JsonProperty("published_at")]
    public DateTime PublishedAt { get; init; }

    [JsonProperty("comment")]
    public string Comment { get; init; } = string.Empty;
}