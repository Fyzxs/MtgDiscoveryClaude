using Newtonsoft.Json;

namespace Cli.Sealed.ImageScraper.MtgJson.Dtos;

internal sealed class MtgJsonIdentifiersDto
{
    [JsonProperty("scgId")]
    public string ScgId { get; init; }

    [JsonProperty("tcgplayerProductId")]
    public string TcgplayerProductId { get; init; }

    [JsonProperty("mcmId")]
    public string McmId { get; init; }

    [JsonProperty("cardtraderId")]
    public string CardTraderId { get; init; }
}
