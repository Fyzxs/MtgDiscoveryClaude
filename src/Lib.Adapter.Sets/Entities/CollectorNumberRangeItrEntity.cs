using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs;
using Newtonsoft.Json;

namespace Lib.Adapter.Sets.Entities;

/// <summary>
/// Adapter-specific implementation of ICollectorNumberRangeItrEntity.
/// </summary>
internal sealed class CollectorNumberRangeItrEntity : ICollectorNumberRangeItrEntity
{
    public string Min { get; set; } = string.Empty;
    public string Max { get; set; } = string.Empty;

    [JsonProperty(nameof(OrConditions))]
    public List<string> InternalOrConditions { get; set; } = [];

    [JsonIgnore]
    public IList<string> OrConditions => InternalOrConditions;
}
