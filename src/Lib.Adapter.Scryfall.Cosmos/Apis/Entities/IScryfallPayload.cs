using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Entities;

public interface IScryfallPayload
{
    [JsonProperty("data")]
    dynamic Data { get; init; }
}
