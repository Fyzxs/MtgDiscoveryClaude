using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;
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

    [JsonProperty("ParsedFilters")]
    public GroupingFiltersItrEntity ParsedFilters { get; set; }

    internal sealed class GroupingFiltersItrEntity : IGroupingFiltersItrEntity
    {
        [JsonProperty("CollectorNumberRange")]
        public CollectorNumberRangeItrEntity InternalCollectorNumberRange { get; set; }

        [JsonProperty("Properties")]
        public Dictionary<string, object> InternalProperties { get; set; }

        [JsonIgnore]
        public ICollectorNumberRangeItrEntity CollectorNumberRange => InternalCollectorNumberRange;
        [JsonIgnore]
        public IDictionary<string, object> Properties => InternalProperties;

    }

    internal sealed class CollectorNumberRangeItrEntity : ICollectorNumberRangeItrEntity
    {
        public string Min { get; set; }
        public string Max { get; set; }

        [JsonProperty("OrConditions")]
        public List<string> InternalOrConditions { get; set; }

        [JsonIgnore]
        public IList<string> OrConditions => InternalOrConditions;
    }
}
