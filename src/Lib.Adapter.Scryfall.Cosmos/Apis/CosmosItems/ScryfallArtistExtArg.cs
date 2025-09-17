using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

public sealed class ScryfallArtistItem : CosmosItem
{
    public override string Id => ArtistId;
    public override string Partition => ArtistId;

    [JsonProperty("artist_id")]
    public string ArtistId { get; set; }
    public ArtistAggregateExtArg Data { get; init; }
}
