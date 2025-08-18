using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Dtos;

public sealed class ArtistAggregateData
{
    [JsonProperty("artist_id")]
    public string ArtistId { get; init; } = string.Empty;

    [JsonProperty("artist_names")]
    public IEnumerable<string> ArtistNames { get; init; } = [];

    [JsonProperty("card_ids")]
    public IEnumerable<string> CardIds { get; init; } = [];

    [JsonProperty("set_ids")]
    public IEnumerable<string> SetIds { get; init; } = [];
}
