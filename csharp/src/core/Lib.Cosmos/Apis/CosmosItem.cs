using System;
using Newtonsoft.Json;

namespace Lib.Cosmos.Apis;

/// <summary>
/// Base class for items stored in Cosmos DB containers.
/// </summary>
public /* cosmos required */ class CosmosItem : CosmosEntity
{

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
    /// Gets or sets the entity tag (ETag) for optimistic concurrency control.
    /// This value is managed by Cosmos DB and should not be manually set.
    /// </summary>
    [JsonProperty("_etag")]
    public string ETag { get; set; }
}
