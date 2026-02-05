using System.Collections.Generic;
using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.RulingItems;

public sealed class RulingAggregateExtEntity : CosmosEntity
{
    [JsonProperty("oracle_id")]
    public string OracleId { get; init; }

    [JsonProperty("rulings")]
    public IEnumerable<RulingEntryExtEntity> Rulings { get; init; }
}
