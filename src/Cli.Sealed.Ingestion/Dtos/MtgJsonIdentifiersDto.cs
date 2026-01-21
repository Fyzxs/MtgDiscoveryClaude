using Newtonsoft.Json;

namespace Cli.Sealed.Ingestion.Dtos;

internal sealed class MtgJsonIdentifiersDto
{
    [JsonProperty("tcgplayerProductId")]
    public string TcgplayerProductId { get; init; }

    [JsonProperty("mcmId")]
    public string McmId { get; init; }

    [JsonProperty("cardTraderId")]
    public string CardTraderId { get; init; }
}
