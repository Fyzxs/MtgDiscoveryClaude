using System.Collections.Generic;
using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;

public sealed class ArtistNameTrigramDataExtEntity : CosmosEntity
{
    [JsonProperty("artist_id")]
    public string ArtistId { get; init; }

    [JsonProperty("name")]
    public string Name { get; init; }

    [JsonProperty("norm")]
    public string Normalized { get; init; }

    [JsonProperty("positions")]
    public ICollection<int> Positions { get; init; }
}
