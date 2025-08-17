using Lib.Cosmos.Apis;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Entities;

public sealed class ScryfallArtistCard : CosmosItem
{
    private readonly string _cardId;
    private readonly string _artistId;

    public ScryfallArtistCard(string cardId, string artistId)
    {
        _cardId = cardId;
        _artistId = artistId;
    }

    public override string Id => _cardId;
    public override string Partition => _artistId;
}