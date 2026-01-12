using Newtonsoft.Json;

namespace Cli.Sealed.Ingestion.Dtos;

internal sealed class MtgJsonPurchaseUrlsDto
{
    [JsonProperty("tcgplayer")]
    public string Tcgplayer { get; init; }

    [JsonProperty("cardmarket")]
    public string Cardmarket { get; init; }

    [JsonProperty("cardKingdom")]
    public string CardKingdom { get; init; }
}
