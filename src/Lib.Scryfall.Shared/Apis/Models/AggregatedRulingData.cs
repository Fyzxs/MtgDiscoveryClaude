using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib.Scryfall.Shared.Apis.Models;

public sealed class AggregatedRulingData
{
    [JsonProperty("oracle_id")]
    public string OracleId { get; init; }

    [JsonProperty("rulings")]
    public IReadOnlyCollection<dynamic> Rulings { get; init; }
}