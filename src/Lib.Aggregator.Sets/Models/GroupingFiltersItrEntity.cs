using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;
using Newtonsoft.Json;

namespace Lib.Aggregator.Sets.Models;

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