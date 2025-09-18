using Lib.Shared.DataModels.Entities.Itrs;
using Newtonsoft.Json;

namespace Lib.Aggregator.Sets.Models;

internal sealed class SetGroupingItrEntity : ISetGroupingItrEntity
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public int Order { get; set; }
    public int CardCount { get; set; }
    public string RawQuery { get; set; }

    [JsonIgnore]
    public IGroupingFiltersItrEntity Filters => ParsedFilters;

    [JsonProperty(nameof(ParsedFilters))]
    public GroupingFiltersItrEntity ParsedFilters { get; set; }
}
