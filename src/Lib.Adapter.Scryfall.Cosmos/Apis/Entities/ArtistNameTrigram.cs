using System.Collections.ObjectModel;
using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Entities;

public sealed class ArtistNameTrigramEntry
{
    [JsonProperty("artistId")]
    public string ArtistId { get; init; } = string.Empty;

    [JsonProperty("name")]
    public string Name { get; init; } = string.Empty;

    [JsonProperty("norm")]
    public string Normalized { get; init; } = string.Empty;

    [JsonProperty("positions")]
    public Collection<int> Positions { get; init; } = new();
}

public sealed class ArtistNameTrigram : CosmosItem
{
    [JsonProperty("id")]
    public override string Id => Trigram;

    [JsonProperty("partition")]
    public override string Partition => Trigram.Substring(0, 1);

    [JsonProperty("trigram")]
    public string Trigram { get; init; } = string.Empty;

    [JsonProperty("artists")]
    public Collection<ArtistNameTrigramEntry> Artists { get; init; } = new();
}