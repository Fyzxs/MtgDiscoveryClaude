using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Nesteds;

public sealed class ArtistAggregateItem
{
    [JsonProperty("artist_id")]
    public string ArtistId { get; init; }

    [JsonProperty("artist_names")]
    public IEnumerable<string> ArtistNames { get; init; }

    [JsonProperty("artist_names_search")]
    public string ArtistNamesSearch { get; init; }

    [JsonProperty("card_ids")]
    public IEnumerable<string> CardIds { get; init; }

    [JsonProperty("set_ids")]
    public IEnumerable<string> SetIds { get; init; }
}
