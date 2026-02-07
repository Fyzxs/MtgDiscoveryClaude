using System.Collections.Generic;
using Lib.Scryfall.Ingestion.Services;

namespace Lib.Scryfall.Ingestion.Tests.Fakes;

internal sealed class SetGroupingsLoaderFake : ISetGroupingsLoader
{
    private readonly Dictionary<string, SetGroupingData> _groupings = [];

    public void AddGrouping(string setCode, SetGroupingData data) => _groupings[setCode] = data;

    public SetGroupingData GetGroupingsForSet(string setCode)
    {
        return _groupings.TryGetValue(setCode, out SetGroupingData data) ? data : null;
    }

    public bool HasGroupingsForSet(string setCode) => _groupings.ContainsKey(setCode);
}
