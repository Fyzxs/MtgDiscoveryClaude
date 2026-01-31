using System;
using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

public sealed class UserInfoExtEntity : CosmosItem
{
    public override string Id => UserId;
    public override string Partition => UserId;

    [JsonProperty("user_id")]
    public string UserId { get; init; }

    [JsonProperty("display_name")]
    public string DisplayName { get; init; }

    [JsonProperty("source_id")]
    public string SourceId { get; init; }

    [JsonProperty("email")]
    public string Email { get; init; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; init; }

    [JsonProperty("last_login_at")]
    public DateTime LastLoginAt { get; init; }
}
