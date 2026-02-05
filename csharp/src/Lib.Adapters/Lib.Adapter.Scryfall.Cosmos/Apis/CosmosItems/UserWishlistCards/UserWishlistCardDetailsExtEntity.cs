using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;

/// <summary>
/// Represents a specific version of a wishlisted card with its finish and quantity.
/// </summary>
public sealed class UserWishlistCardDetailsExtEntity : CosmosEntity
{
    /// <summary>
    /// The finish type of the card (e.g., "nonfoil", "foil", "etched").
    /// </summary>
    [JsonProperty("finish")]
    public string Finish { get; init; }

    /// <summary>
    /// Any special characteristics of this card version (e.g., "none", "signed", "altered", "proof").
    /// </summary>
    [JsonProperty("special")]
    public string Special { get; init; }

    /// <summary>
    /// The number of cards of this specific version wanted by the user.
    /// </summary>
    [JsonProperty("count")]
    public int Count { get; init; }
}
