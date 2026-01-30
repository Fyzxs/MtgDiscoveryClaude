using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

public interface IScryfallPayload
{
    [JsonProperty("data")]
    dynamic Data { get; init; }
}
