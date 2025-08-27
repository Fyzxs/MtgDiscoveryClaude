using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Lib.Scryfall.Ingestion.Services;

internal interface ISetGroupingsLoader
{
    SetGroupingData GetGroupingsForSet(string setCode);
    bool HasGroupingsForSet(string setCode);
}

internal sealed class SetGroupingsLoader : ISetGroupingsLoader
{
    private readonly Dictionary<string, SetGroupingData> _groupings;

    public SetGroupingsLoader(string filePath)
    {
        _groupings = LoadGroupings(filePath);
    }

    private Dictionary<string, SetGroupingData> LoadGroupings(string filePath)
    {
        if (File.Exists(filePath) is false)
        {
            return new Dictionary<string, SetGroupingData>();
        }

        string json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<Dictionary<string, SetGroupingData>>(json)
               ?? new Dictionary<string, SetGroupingData>();
    }

    public SetGroupingData GetGroupingsForSet(string setCode)
    {
        return _groupings.TryGetValue(setCode, out SetGroupingData grouping)
            ? grouping
            : null;
    }

    public bool HasGroupingsForSet(string setCode)
    {
        return _groupings.ContainsKey(setCode);
    }
}

internal sealed class SetGroupingData
{
    public string SetCode { get; set; }
    public List<CardGrouping> Groupings { get; set; }
}

internal sealed class CardGrouping
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public int Order { get; set; }
    public int CardCount { get; set; }
    public string RawQuery { get; set; }
    public GroupingFilters ParsedFilters { get; set; }
}

internal sealed class GroupingFilters
{
    public CollectorNumberRange CollectorNumberRange { get; set; }
    public Dictionary<string, object> Properties { get; set; }
}

internal sealed class CollectorNumberRange
{
    public string Min { get; set; }
    public string Max { get; set; }
}
