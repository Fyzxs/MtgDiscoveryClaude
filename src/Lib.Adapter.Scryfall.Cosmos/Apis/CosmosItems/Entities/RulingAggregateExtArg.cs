using System.Collections.Generic;
using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;

public sealed class RulingAggregateExtArg : CosmosEntity
{
    [JsonProperty("oracle_id")]
    public string OracleId { get; init; }

    [JsonProperty("rulings")]
    public IEnumerable<RulingEntryExtArg> Rulings { get; init; }
}
