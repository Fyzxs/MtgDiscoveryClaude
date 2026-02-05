using System.Collections.Generic;
using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.ArtistNameTrigrams;

public sealed class ArtistNameTrigramExtEntity : CosmosItem
{
    [JsonProperty("id")]
    public override string Id => Trigram;

    [JsonProperty("partition")]
    public override string Partition => Trigram[..1];

    [JsonProperty("trigram")]
    public string Trigram { get; init; }

    [JsonProperty("artists")]
    public ICollection<ArtistNameTrigramDataExtEntity> Artists { get; init; }
}
