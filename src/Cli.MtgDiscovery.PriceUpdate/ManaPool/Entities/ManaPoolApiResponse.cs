using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cli.MtgDiscovery.PriceUpdate.ManaPool.Entities;

internal sealed class ManaPoolApiResponse
{
    [JsonProperty("meta")]
    public ManaPoolMeta Meta { get; init; } = new();

    [JsonProperty("data")]
    public IReadOnlyList<ManaPoolPriceItem> Data { get; init; } = [];
}
