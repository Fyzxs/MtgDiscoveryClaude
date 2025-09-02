using System.Collections.Generic;
using System.Linq;
using Lib.Scryfall.Ingestion.Apis.Aggregation;

namespace Lib.Scryfall.Ingestion.Aggregation;

internal sealed class CardNameTrigramAggregate : ICardNameTrigramAggregate
{
    private readonly string _trigram;
    private readonly Dictionary<string, CardNameTrigramEntryImpl> _entries;

    public CardNameTrigramAggregate(string trigram)
    {
        _trigram = trigram;
        _entries = new Dictionary<string, CardNameTrigramEntryImpl>();
    }

    public string Trigram() => _trigram;
    public IEnumerable<ICardNameTrigramEntry> Entries() => _entries.Values;

    public void AddCard(string name, string normalized, int position)
    {
        if (_entries.TryGetValue(name, out CardNameTrigramEntryImpl entry) is false)
        {
            entry = new CardNameTrigramEntryImpl(name, normalized);
            _entries[name] = entry;
        }
        entry.AddPosition(position);
    }

    private sealed class CardNameTrigramEntryImpl : ICardNameTrigramEntry
    {
        private readonly string _name;
        private readonly string _normalized;
        private readonly HashSet<int> _positions;

        public CardNameTrigramEntryImpl(string name, string normalized)
        {
            _name = name;
            _normalized = normalized;
            _positions = new HashSet<int>();
        }

        public string Name() => _name;
        public string Normalized() => _normalized;
        public IEnumerable<int> Positions() => _positions.OrderBy(p => p);

        public void AddPosition(int position) => _positions.Add(position);
    }
}