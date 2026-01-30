using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib.Scryfall.Ingestion.Dtos;

public sealed class ExtScryfallRulingsDto
{
    [JsonProperty("oracle_id")]
    public string OracleId { get; init; }

    [JsonProperty("rulings")]
    public IReadOnlyCollection<dynamic> Rulings { get; init; }
}
