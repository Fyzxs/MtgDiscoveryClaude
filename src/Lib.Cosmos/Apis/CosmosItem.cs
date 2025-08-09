using System;
using Newtonsoft.Json;

namespace Lib.Cosmos.Apis;

/// <summary>
/// Base class for items stored in Cosmos DB containers.
/// </summary>
public /* cosmos required */ class CosmosItem
{
    private string _itemType;

    /// <summary>
    /// Gets or sets the unique identifier for the item.
    /// </summary>
    [JsonProperty("id")]
    public virtual string Id { get; set; }

    /// <summary>
    /// Gets or sets the partition key value for the item.
    /// </summary>
    [JsonProperty("partition")]
    public virtual string Partition { get; set; }

    /// <summary>
    /// Gets or sets the creation date of the item in ISO 8601 format.
    /// </summary>
    [JsonProperty("created_date")]
    public string CreatedDate { get; set; } = DateTime.UtcNow.ToString("O");

    /// <summary>
    /// Gets or sets the type of the item.
    /// </summary>
    [JsonProperty("item_type")]
    public string ItemType
    {
        get => _itemType ??= GetType().FullName;
        set => _itemType = value;
    }
}
