using System.Collections.Generic;
using Lib.Scryfall.Ingestion.Apis.Aggregation;

namespace Lib.Scryfall.Ingestion.Aggregation;

internal sealed class ArtistAggregate : IArtistAggregate
{
    private readonly string _artistId;
    private readonly HashSet<string> _cardIds;
    private readonly HashSet<string> _setIds;
    private readonly HashSet<string> _artistNames;

    public ArtistAggregate(string artistId)
    {
        _artistId = artistId;
        _cardIds = [];
        _setIds = [];
        _artistNames = [];
    }

    public string ArtistId() => _artistId;
    public IEnumerable<string> ArtistNames() => _artistNames;
    public IEnumerable<string> CardIds() => _cardIds;
    public IEnumerable<string> SetIds() => _setIds;

    public void AddName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return;
        _artistNames.Add(name);
    }

    public void AddCard(string cardId)
    {
        if (string.IsNullOrWhiteSpace(cardId)) return;
        _cardIds.Add(cardId);
    }

    public void AddSet(string setId)
    {
        if (string.IsNullOrWhiteSpace(setId)) return;
        _setIds.Add(setId);
    }
}
