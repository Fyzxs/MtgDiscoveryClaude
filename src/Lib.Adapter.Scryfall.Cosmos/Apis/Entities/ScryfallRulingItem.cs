using Lib.Cosmos.Apis;
using Lib.Scryfall.Shared.Apis.Models;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Entities;

public sealed class ScryfallRulingItem : CosmosItem
{
    public override string Id => Data.OracleId;

    public override string Partition => Data.OracleId;

    [JsonProperty("data")]
    public AggregatedRulingData Data { get; init; }
}
