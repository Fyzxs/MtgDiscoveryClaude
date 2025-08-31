using System.Collections.Generic;
using System.Linq;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Storage;

internal sealed class BulkSetStorage : IBulkSetStorage
{
    private readonly Dictionary<string, BulkSetData> _sets = new();

    public void Add(IScryfallSet set)
    {
        if (_sets.ContainsKey(set.Id()) is false)
        {
            _sets[set.Id()] = new BulkSetData(set);
        }
    }

    public void AddCard(string setId, string cardId)
    {
        if (_sets.TryGetValue(setId, out BulkSetData setData))
        {
            setData.CardIds.Add(cardId);
        }
    }

    public IScryfallSet Get(string setId)
    {
        return _sets.TryGetValue(setId, out BulkSetData setData)
            ? setData.Set
            : null;
    }

    public IEnumerable<IScryfallSet> GetAll()
    {
        return _sets.Values.Select(data => data.Set);
    }

    public bool Contains(string setId)
    {
        return _sets.ContainsKey(setId);
    }

    private sealed class BulkSetData
    {
        public IScryfallSet Set { get; }
        public HashSet<string> CardIds { get; }

        public BulkSetData(IScryfallSet set)
        {
            Set = set;
            CardIds = new HashSet<string>();
        }
    }
}