using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

public sealed class UserSealedProductExtEntity : CosmosItem
{
    [JsonProperty("id")]
    public override string Id => ProductUuid;

    [JsonProperty("partition")]
    public override string Partition => CollectionId;

    [JsonProperty("collection_id")]
    public string CollectionId { get; init; }

    [JsonProperty("product_uuid")]
    public string ProductUuid { get; init; }

    [JsonProperty("count")]
    public int Count { get; init; }

    [JsonProperty("product_name")]
    public string ProductName { get; init; }

    [JsonProperty("set_code")]
    public string SetCode { get; init; }

    [JsonProperty("category")]
    public string Category { get; init; }

    [JsonProperty("image_url")]
    public string ImageUrl { get; init; }

    [JsonProperty("updated_at")]
    public string UpdatedAt { get; init; }
}
