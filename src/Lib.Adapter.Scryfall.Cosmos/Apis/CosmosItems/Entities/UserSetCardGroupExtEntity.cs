using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;

public sealed class UserSetCardGroupExtEntity
{
    [JsonProperty("non_foil")]
    public UserSetCardFinishGroupExtEntity NonFoil { get; init; } = new();

    [JsonProperty("foil")]
    public UserSetCardFinishGroupExtEntity Foil { get; init; } = new();

    [JsonProperty("etched")]
    public UserSetCardFinishGroupExtEntity Etched { get; init; } = new();
}