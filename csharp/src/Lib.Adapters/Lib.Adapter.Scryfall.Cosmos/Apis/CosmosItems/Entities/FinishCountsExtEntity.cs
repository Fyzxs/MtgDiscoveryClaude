using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;

public sealed class FinishCountsExtEntity
{
    [JsonProperty("total")]
    public int Total { get; init; }

    [JsonProperty("non_foil")]
    public int NonFoil { get; init; }

    [JsonProperty("foil")]
    public int Foil { get; init; }

    [JsonProperty("etched")]
    public int Etched { get; init; }
}
