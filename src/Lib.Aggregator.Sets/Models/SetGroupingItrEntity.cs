using Lib.Shared.DataModels.Entities.Itrs;
using Newtonsoft.Json;

namespace Lib.Aggregator.Sets.Models;

internal sealed class SetGroupingItrEntity : ISetGroupingItrEntity
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("displayName")]
    public string DisplayName { get; set; }

    [JsonProperty("order")]
    public int Order { get; set; }

    [JsonProperty("rawQuery")]
    public string RawQuery { get; set; }

    [JsonProperty("parsedFilters")]
    public GroupingFiltersItrEntity Filters { get; set; }

    [JsonProperty("cardCounts")]
    public FinishCountsOufEntity CardCounts { get; set; }

    IGroupingFiltersItrEntity ISetGroupingItrEntity.Filters => Filters;

    IFinishCountsOufEntity ISetGroupingItrEntity.CardCounts => CardCounts;
}
