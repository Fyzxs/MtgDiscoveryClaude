using Newtonsoft.Json;

namespace Lib.Cosmos.Apis;

public /* cosmos required */ class CosmosEntity
{
    /// <summary>
    /// Gets or sets the type of the item.
    /// </summary>
    [JsonProperty("item_type")]
    public string ItemType
    {
        get => GetType().FullName;
        set => _ = value;
    }
}
