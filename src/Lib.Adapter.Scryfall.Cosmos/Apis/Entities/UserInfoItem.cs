using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Entities;

public sealed class UserInfoItem : CosmosItem
{
    public override string Id => UserId;
    public override string Partition => UserId;

    [JsonProperty("user_id")]
    public string UserId { get; init; } = string.Empty;

    [JsonProperty("display_name")]
    public string DisplayName { get; init; } = string.Empty;

    [JsonProperty("source_id")]
    public string SourceId { get; init; } = string.Empty;
}