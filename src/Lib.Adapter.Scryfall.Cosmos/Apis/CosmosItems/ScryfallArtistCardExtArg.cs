using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

public sealed class ScryfallArtistCardExtArg : CosmosItem, IScryfallPayload
{
    public override string Id => Data.id;
    public override string Partition => ArtistId;

    [JsonProperty("artist_id")]
    public string ArtistId { get; init; }
    public dynamic Data { get; init; }
}
