using System.Collections.Generic;
using System.Linq;
using Lib.Scryfall.Ingestion.Apis.Aggregation;

namespace Lib.Scryfall.Ingestion.Aggregation;

internal sealed class ArtistTrigramAggregate : IArtistTrigramAggregate
{
    private readonly string _trigram;
    private readonly Dictionary<string, ArtistTrigramEntryImpl> _entries;

    public ArtistTrigramAggregate(string trigram)
    {
        _trigram = trigram;
        _entries = [];
    }

    public string Trigram() => _trigram;
    public IEnumerable<IArtistTrigramEntry> Entries() => _entries.Values;

    public void AddArtist(string artistId, string name, string normalized, int position)
    {
        string key = $"{artistId}:{name}";
        if (_entries.TryGetValue(key, out ArtistTrigramEntryImpl entry) is false)
        {
            entry = new ArtistTrigramEntryImpl(artistId, name, normalized);
            _entries[key] = entry;
        }
        entry.AddPosition(position);
    }

    private sealed class ArtistTrigramEntryImpl : IArtistTrigramEntry
    {
        private readonly string _artistId;
        private readonly string _name;
        private readonly string _normalized;
        private readonly HashSet<int> _positions;

        public ArtistTrigramEntryImpl(string artistId, string name, string normalized)
        {
            _artistId = artistId;
            _name = name;
            _normalized = normalized;
            _positions = [];
        }

        public string ArtistId() => _artistId;
        public string Name() => _name;
        public string Normalized() => _normalized;
        public IEnumerable<int> Positions() => _positions.OrderBy(p => p);

        public void AddPosition(int position) => _positions.Add(position);
    }
}
