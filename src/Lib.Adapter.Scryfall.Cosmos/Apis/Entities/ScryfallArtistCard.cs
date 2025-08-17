using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Entities;

public sealed class ScryfallArtistCard : CosmosItem, IScryfallPayload
{
    private readonly string _artistId;

    public ScryfallArtistCard(string artistId, dynamic data)
    {
        _artistId = artistId;
        Data = data;
    }

    public override string Id => Data.id;
    public override string Partition => _artistId;

    [JsonProperty("data")]
    public dynamic Data { get; init; }
}