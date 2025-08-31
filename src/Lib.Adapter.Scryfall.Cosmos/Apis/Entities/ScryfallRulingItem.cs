using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Entities;

public sealed class ScryfallRulingItem : CosmosItem
{
    [JsonProperty("id")]
    public override string Id => Data?.OracleId ?? string.Empty;

    [JsonProperty("partition")]
    public override string Partition => Data?.OracleId ?? string.Empty;

    [JsonProperty("data")]
    public dynamic Data { get; init; }
}