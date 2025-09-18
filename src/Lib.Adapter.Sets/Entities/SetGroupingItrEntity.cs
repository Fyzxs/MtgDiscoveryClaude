using Lib.Shared.DataModels.Entities.Itrs;
using Newtonsoft.Json;

namespace Lib.Adapter.Sets.Entities;

/// <summary>
/// Adapter-specific implementation of ISetGroupingItrEntity.
/// </summary>
internal sealed class SetGroupingItrEntity : ISetGroupingItrEntity
{
    public string Id { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public int Order { get; set; }
    public int CardCount { get; set; }
    public string RawQuery { get; set; } = string.Empty;

    [JsonIgnore]
    public IGroupingFiltersItrEntity Filters => ParsedFilters;

    [JsonProperty(nameof(ParsedFilters))]
    public GroupingFiltersItrEntity ParsedFilters { get; set; } = new();
}
