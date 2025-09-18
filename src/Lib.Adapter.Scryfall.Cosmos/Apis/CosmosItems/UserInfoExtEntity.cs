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
}
