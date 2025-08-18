using System.Collections.Generic;
using Lib.Scryfall.Ingestion.Apis.Aggregation;

namespace Lib.Scryfall.Ingestion.Aggregation;

internal sealed class ArtistAggregate : IArtistAggregate
{
    private readonly string _artistId;
    private readonly HashSet<string> _cardIds;
    private readonly HashSet<string> _setIds;
    private readonly HashSet<string> _artistNames;
    private bool _isDirty;

    public ArtistAggregate(string artistId)
    {
        _artistId = artistId;
        _cardIds = [];
        _setIds = [];
        _artistNames = [];
        _isDirty = false;
    }

    public string ArtistId() => _artistId;
    public IEnumerable<string> ArtistNames() => _artistNames;
    public IEnumerable<string> CardIds() => _cardIds;
    public IEnumerable<string> SetIds() => _setIds;
    public bool IsDirty() => _isDirty;
    public void MarkClean() => _isDirty = false;

    public void AddName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return;
        if (_artistNames.Add(name))
        {
            _isDirty = true;
        }
    }

    public void AddCard(string cardId)
    {
        if (string.IsNullOrWhiteSpace(cardId)) return;
        if (_cardIds.Add(cardId))
        {
            _isDirty = true;
        }
    }

    public void AddSet(string setId)
    {
        if (string.IsNullOrWhiteSpace(setId)) return;
        if (_setIds.Add(setId))
        {
            _isDirty = true;
        }
    }
}
