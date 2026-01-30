using System.Collections.ObjectModel;
using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;

public sealed class CardNameTrigramDataExtEntity : CosmosEntity
{
    [JsonProperty("name")]
    public string Name { get; init; }

    [JsonProperty("norm")]
    public string Normalized { get; init; }

    [JsonProperty("positions")]
    public Collection<int> Positions { get; init; }
}
