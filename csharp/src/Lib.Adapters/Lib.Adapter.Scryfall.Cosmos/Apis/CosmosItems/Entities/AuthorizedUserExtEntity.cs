using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;

public sealed class AuthorizedUserExtEntity
{
    [JsonProperty("user_id")]
    public string UserId { get; init; }

    [JsonProperty("role")]
    public string Role { get; init; }

    [JsonProperty("granted_at")]
    public string GrantedAt { get; init; }

    [JsonProperty("granted_by")]
    public string GrantedBy { get; init; }
}
