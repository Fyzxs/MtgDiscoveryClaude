using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

public sealed class CollectionExtEntity : CosmosItem
{
    public override string Id => CollectionId;
    public override string Partition => OwnerId;

    [JsonProperty("collection_id")]
    public string CollectionId { get; init; }

    [JsonProperty("owner_id")]
    public string OwnerId { get; init; }

    [JsonProperty("name")]
    public string Name { get; init; }

    [JsonProperty("type")]
    public string Type { get; init; }

    [JsonProperty("visibility")]
    public string Visibility { get; init; }

    [JsonProperty("is_default")]
    public bool IsDefault { get; init; }

    [JsonProperty("authorized_users")]
    public IEnumerable<AuthorizedUserExtEntity> AuthorizedUsers { get; init; } = [];

    [JsonProperty("created_at")]
    public string CreatedAt { get; init; }

    [JsonProperty("updated_at")]
    public string UpdatedAt { get; init; }
}
