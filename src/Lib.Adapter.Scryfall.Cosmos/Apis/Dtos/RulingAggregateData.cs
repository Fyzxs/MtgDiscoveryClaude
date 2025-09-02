using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Dtos;

public sealed class RulingAggregateData
{
    [JsonProperty("oracle_id")]
    public string OracleId { get; init; } = string.Empty;

    [JsonProperty("rulings")]
    public IEnumerable<RulingEntry> Rulings { get; init; } = [];
}