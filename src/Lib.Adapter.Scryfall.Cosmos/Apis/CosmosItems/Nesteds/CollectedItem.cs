using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Nesteds;

public sealed class CollectedItem
{
    [JsonProperty("finish")]
    public string Finish { get; init; }

    [JsonProperty("special")]
    public string Special { get; init; }

    [JsonProperty("count")]
    public int Count { get; init; }
}
