using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;
using Newtonsoft.Json;

namespace Lib.Aggregator.Sets.Models;

internal sealed class GroupingFiltersItrEntity : IGroupingFiltersItrEntity
{
    [JsonProperty(nameof(CollectorNumberRange))]
    public CollectorNumberRangeItrEntity InternalCollectorNumberRange { get; set; }

    [JsonProperty(nameof(Properties))]
    public Dictionary<string, object> InternalProperties { get; set; }

    [JsonIgnore]
    public ICollectorNumberRangeItrEntity CollectorNumberRange => InternalCollectorNumberRange;
    [JsonIgnore]
    public IDictionary<string, object> Properties => InternalProperties;

}
