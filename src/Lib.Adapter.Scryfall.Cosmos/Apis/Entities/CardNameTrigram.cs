using System.Collections.ObjectModel;
using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Entities;

public sealed class CardNameTrigramEntry
{
    [JsonProperty("name")]
    public string Name { get; init; } = string.Empty;

    [JsonProperty("norm")]
    public string Normalized { get; init; } = string.Empty;

    [JsonProperty("positions")]
    public Collection<int> Positions { get; init; } = new();
}

public sealed class CardNameTrigram : CosmosItem
{
    [JsonProperty("id")]
    public override string Id => Trigram;

    [JsonProperty("partition")]
    public override string Partition => Trigram.Substring(0, 1);

    [JsonProperty("trigram")]
    public string Trigram { get; init; } = string.Empty;

    [JsonProperty("cards")]
    public Collection<CardNameTrigramEntry> Cards { get; init; } = new();
}