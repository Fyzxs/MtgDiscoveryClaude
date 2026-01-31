#pragma warning disable CA1819
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib.MtgDiscovery.Entry.Entities.Outs.Artists;

public sealed class ScryfallArtistOutEntity
{
    [JsonProperty("artist_id")]
    public string ArtistId { get; set; }

    [JsonProperty("artist_names")]
    public ICollection<string> ArtistNames { get; set; }

    [JsonProperty("artist_names_search")]
    public string ArtistNamesSearch { get; set; }

    [JsonProperty("card_ids")]
    public ICollection<string> CardIds { get; set; }

    [JsonProperty("set_ids")]
    public ICollection<string> SetIds { get; set; }
}
