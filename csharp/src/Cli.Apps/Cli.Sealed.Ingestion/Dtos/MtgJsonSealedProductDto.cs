using Newtonsoft.Json;

namespace Cli.Sealed.Ingestion.Dtos;

internal sealed class MtgJsonSealedProductDto
{
    [JsonProperty("uuid")]
    public string Uuid { get; init; }

    [JsonProperty("name")]
    public string Name { get; init; }

    [JsonProperty("category")]
    public string Category { get; init; }

    [JsonProperty("subtype")]
    public string Subtype { get; init; }

    [JsonProperty("cardCount")]
    public int? CardCount { get; init; }

    [JsonProperty("releaseDate")]
    public string ReleaseDate { get; init; }

    [JsonProperty("identifiers")]
    public MtgJsonIdentifiersDto Identifiers { get; init; }

    [JsonProperty("purchaseUrls")]
    public MtgJsonPurchaseUrlsDto PurchaseUrls { get; init; }
}
