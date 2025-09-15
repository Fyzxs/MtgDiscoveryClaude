using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Nesteds;

/// <summary>
/// Represents a specific version of a collected card with its finish and quantity.
/// </summary>
public sealed class CollectedItem
{
    /// <summary>
    /// The finish type of the card (e.g., "nonfoil", "foil", "etched").
    /// </summary>
    [JsonProperty("finish")]
    public string Finish { get; init; }

    /// <summary>
    /// Any special characteristics of this card version (e.g., "promo", "extended art").
    /// </summary>
    [JsonProperty("special")]
    public string Special { get; init; }

    /// <summary>
    /// The number of cards of this specific version owned by the user.
    /// </summary>
    [JsonProperty("count")]
    public int Count { get; init; }
}
