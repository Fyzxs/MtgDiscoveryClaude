using Newtonsoft.Json;

namespace Cli.MtgDiscovery.PriceUpdate.ManaPool.Entities;

internal sealed class ManaPoolPriceItem
{
    [JsonProperty("scryfall_id")]
    public string ScryfallId { get; init; } = string.Empty;

    [JsonProperty("price_cents")]
    public int? PriceCents { get; init; }

    [JsonProperty("price_cents_foil")]
    public int? PriceCentsFoil { get; init; }

    [JsonProperty("price_cents_etched")]
    public int? PriceCentsEtched { get; init; }
}
