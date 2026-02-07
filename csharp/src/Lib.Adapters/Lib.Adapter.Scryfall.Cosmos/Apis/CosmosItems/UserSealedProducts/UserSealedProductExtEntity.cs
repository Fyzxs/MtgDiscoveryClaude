using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSealedProducts;

public sealed class UserSealedProductExtEntity : CosmosItem
{
    [JsonProperty("id")]
    public override string Id => ProductUuid;

    [JsonProperty("partition")]
    public override string Partition => UserId;

    [JsonProperty("user_id")]
    public string UserId { get; init; }

    [JsonProperty("product_uuid")]
    public string ProductUuid { get; init; }

    [JsonProperty("set_id")]
    public string SetId { get; init; }

    [JsonProperty("count")]
    public int Count { get; init; }

    [JsonProperty("product_name")]
    public string ProductName { get; init; }

    [JsonProperty("set_code")]
    public string SetCode { get; init; }

    [JsonProperty("category")]
    public string Category { get; init; }

    [JsonProperty("subtype")]
    public string Subtype { get; init; }

    [JsonProperty("image_url")]
    public string ImageUrl { get; init; }

    [JsonProperty("release_date")]
    public string ReleaseDate { get; init; }

    [JsonProperty("set_name")]
    public string SetName { get; init; }

    [JsonProperty("tcgplayer_product_id")]
    public string TcgplayerProductId { get; init; }

    [JsonProperty("purchase_url_tcgplayer")]
    public string PurchaseUrlTcgplayer { get; init; }

    [JsonProperty("purchase_url_cardmarket")]
    public string PurchaseUrlCardmarket { get; init; }

    [JsonProperty("purchase_url_cardkingdom")]
    public string PurchaseUrlCardKingdom { get; init; }

    [JsonProperty("card_count")]
    public int? CardCount { get; init; }

    [JsonProperty("updated_at")]
    public string UpdatedAt { get; init; }
}
