using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs;
using Newtonsoft.Json;

namespace Lib.Aggregator.Sets.Models;

internal sealed class CollectorNumberRangeItrEntity : ICollectorNumberRangeItrEntity
{
    public string Min { get; set; }
    public string Max { get; set; }

    [JsonProperty(nameof(OrConditions))]
    public List<string> InternalOrConditions { get; set; }

    [JsonIgnore]
    public IList<string> OrConditions => InternalOrConditions;
}
