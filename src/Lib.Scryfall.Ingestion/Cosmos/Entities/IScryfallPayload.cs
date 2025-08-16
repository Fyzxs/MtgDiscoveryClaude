using Newtonsoft.Json;

namespace Lib.Scryfall.Ingestion.Cosmos.Entities;

internal interface IScryfallPayload
{
    [JsonProperty("data")]
    dynamic Data { get; init; }
}
