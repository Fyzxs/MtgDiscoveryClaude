using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib.Scryfall.Shared.Apis.Models;

//TODO: This doesn't belong here... but brain no brain right now
public sealed class AggregatedRulingData
{
    [JsonProperty("oracle_id")]
    public string OracleId { get; init; }

    [JsonProperty("rulings")]
    public IReadOnlyCollection<dynamic> Rulings { get; init; }
}
