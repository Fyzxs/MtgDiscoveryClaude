using Lib.Adapter.Scryfall.Cosmos.Apis.Dtos;
using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Entities;

public sealed class ScryfallArtistItem : CosmosItem
{
    private readonly string _artistId;

    public ScryfallArtistItem(string artistId, ArtistAggregateData data)
    {
        _artistId = artistId;
        Data = data;
    }

    public override string Id => _artistId;
    public override string Partition => _artistId;

    [JsonProperty("data")]
    public ArtistAggregateData Data { get; init; }
}