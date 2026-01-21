using Newtonsoft.Json;

namespace Cli.Sealed.ImageScraper.MtgJson.Dtos;

internal sealed class MtgJsonSealedProductDto
{
    [JsonProperty("uuid")]
    public string Uuid { get; init; }

    [JsonProperty("name")]
    public string Name { get; init; }

    [JsonProperty("category")]
    public string Category { get; init; }

    [JsonProperty("identifiers")]
    public MtgJsonIdentifiersDto Identifiers { get; init; }
}
