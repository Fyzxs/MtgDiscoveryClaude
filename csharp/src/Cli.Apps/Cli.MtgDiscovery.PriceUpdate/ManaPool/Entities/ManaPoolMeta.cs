using Newtonsoft.Json;

namespace Cli.MtgDiscovery.PriceUpdate.ManaPool.Entities;

internal sealed class ManaPoolMeta
{
    [JsonProperty("page")]
    public int Page { get; init; }

    [JsonProperty("page_size")]
    public int PageSize { get; init; }

    [JsonProperty("total_pages")]
    public int TotalPages { get; init; }

    [JsonProperty("total_count")]
    public int TotalCount { get; init; }
}
