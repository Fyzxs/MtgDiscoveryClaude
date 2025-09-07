using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

public sealed class ScryfallArtistSetItem : CosmosItem
{
    public override string Id => SetId;
    public override string Partition => ArtistId;

    [JsonProperty("artist_id")]
    public string ArtistId { get; set; }

    [JsonProperty("set_id")]
    public string SetId { get; set; }
}
