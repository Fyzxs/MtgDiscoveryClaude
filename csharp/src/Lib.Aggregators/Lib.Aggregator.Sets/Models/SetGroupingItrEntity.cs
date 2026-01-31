using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
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
    public FinishCountsItrEntity CardCounts { get; set; }

    IGroupingFiltersItrEntity ISetGroupingItrEntity.Filters => Filters;

    IFinishCountsItrEntity ISetGroupingItrEntity.CardCounts => CardCounts;
}
