using System.Collections.Generic;

namespace Lib.Scryfall.Ingestion.Dtos;

public sealed class ExtScryfallRulingsDto
{
    [JsonProperty("oracle_id")]
    public string OracleId { get; init; }

    [JsonProperty("rulings")]
    public IReadOnlyCollection<dynamic> Rulings { get; init; }
}
