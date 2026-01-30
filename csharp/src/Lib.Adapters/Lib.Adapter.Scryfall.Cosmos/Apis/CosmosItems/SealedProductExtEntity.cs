using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

public sealed class SealedProductExtEntity : CosmosItem
{
    public override string Id => Uuid;
    public override string Partition => SetId;

    [JsonProperty("uuid")]
    public string Uuid { get; init; }

    [JsonProperty("set_id")]
    public string SetId { get; init; }

    [JsonProperty("set_code")]
    public string SetCode { get; init; }

    [JsonProperty("set_name")]
    public string SetName { get; init; }

    [JsonProperty("name")]
    public string Name { get; init; }

    [JsonProperty("category")]
    public string Category { get; init; }

    [JsonProperty("subtype")]
    public string Subtype { get; init; }

    [JsonProperty("card_count")]
    public int? CardCount { get; init; }

    [JsonProperty("release_date")]
    public string ReleaseDate { get; init; }

    [JsonProperty("tcgplayer_product_id")]
    public string TcgplayerProductId { get; init; }

    [JsonProperty("mcm_id")]
    public string McmId { get; init; }

    [JsonProperty("cardtrader_id")]
    public string CardtraderId { get; init; }

    [JsonProperty("image_url")]
    public string ImageUrl { get; init; }

    [JsonProperty("purchase_url_tcgplayer")]
    public string PurchaseUrlTcgplayer { get; init; }

    [JsonProperty("purchase_url_cardmarket")]
    public string PurchaseUrlCardmarket { get; init; }

    [JsonProperty("purchase_url_cardkingdom")]
    public string PurchaseUrlCardKingdom { get; init; }
}
