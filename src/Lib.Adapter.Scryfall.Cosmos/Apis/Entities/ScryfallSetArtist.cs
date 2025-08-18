using Lib.Cosmos.Apis;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Entities;

public sealed class ScryfallSetArtist : CosmosItem
{
    private readonly string _artistId;
    private readonly string _setId;

    public ScryfallSetArtist(string artistId, string setId)
    {
        _artistId = artistId;
        _setId = setId;
    }

    public override string Id => _artistId;
    public override string Partition => _setId;
}
