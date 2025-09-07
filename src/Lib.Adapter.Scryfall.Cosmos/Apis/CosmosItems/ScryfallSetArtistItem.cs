using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

public sealed class ScryfallSetArtistItem : CosmosItem
{
    public override string Id => ArtistId;
    public override string Partition => SetId;

    [JsonProperty("artist_id")]
    public string ArtistId { get; set; }

    [JsonProperty("set_id")]
    public string SetId { get; set; }
}
