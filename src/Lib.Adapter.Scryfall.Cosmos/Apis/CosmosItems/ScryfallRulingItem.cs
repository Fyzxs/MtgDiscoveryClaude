using System.Collections.Generic;
using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

public sealed class ScryfallRulingItem : CosmosItem
{
    public override string Id => OracleId;

    public override string Partition => OracleId;

    [JsonProperty("oracle_id")]
    public string OracleId { get; init; }

    [JsonProperty("data")]
    public ICollection<dynamic> Data { get; init; }
}
