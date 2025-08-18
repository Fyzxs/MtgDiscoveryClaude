using Lib.Cosmos.Apis;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Entities;

public sealed class ScryfallArtistSet : CosmosItem
{
    private readonly string _setId;
    private readonly string _artistId;

    public ScryfallArtistSet(string setId, string artistId)
    {
        _setId = setId;
        _artistId = artistId;
    }

    public override string Id => _setId;
    public override string Partition => _artistId;
}
