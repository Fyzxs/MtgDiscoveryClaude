using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib.Scryfall.Shared.Apis.Models;

public interface IScryfallRuling
{
    [JsonProperty("oracle_id")]
    string OracleId { get; }

    [JsonProperty("rulings")]
    ICollection<dynamic> Rulings { get; }
}
