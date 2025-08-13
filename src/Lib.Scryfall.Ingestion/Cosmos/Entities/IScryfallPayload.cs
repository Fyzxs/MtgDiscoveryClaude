using Newtonsoft.Json;

namespace Lib.Scryfall.Ingestion.Cosmos.Entities;

public interface IScryfallPayload
{
    [JsonProperty("data")]
    dynamic Data { get; init; }
}
