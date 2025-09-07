using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib.Scryfall.Shared.Apis.Models;

public sealed class AggregatedRulingData : IAggregatedRulingData
{
    public string OracleId { get; init; }
    public IReadOnlyCollection<dynamic> Rulings { get; init; }
}

public interface IAggregatedRulingData
{
    [JsonProperty("oracle_id")]
    string OracleId { get; }

    [JsonProperty("rulings")]
    IReadOnlyCollection<dynamic> Rulings { get; }
}
