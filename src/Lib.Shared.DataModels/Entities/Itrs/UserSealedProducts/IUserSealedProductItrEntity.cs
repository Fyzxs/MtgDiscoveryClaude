namespace Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;

/// <summary>
/// Represents a user's sealed product collection entry following MicroObjects principles.
/// Contains all information needed to manage a user's ownership of a specific sealed product.
/// </summary>
public interface IUserSealedProductItrEntity : Abstractions.IItrEntity
{
    /// <summary>
    /// The unique identifier of the user who owns this sealed product collection entry.
    /// </summary>
    string UserId { get; }

    /// <summary>
    /// The unique identifier (UUID) of the sealed product in the collection.
    /// </summary>
    string ProductUuid { get; }

    /// <summary>
    /// The name of the sealed product (denormalized for display without additional lookups).
    /// </summary>
    string ProductName { get; }

    /// <summary>
    /// The code of the set this sealed product belongs to (denormalized for URLs and display).
    /// </summary>
    string SetCode { get; }

    /// <summary>
    /// The category of the sealed product (e.g., "booster_box", "draft_booster", "bundle").
    /// </summary>
    string Category { get; }

    /// <summary>
    /// The URL to the product image (denormalized for display without additional lookups).
    /// </summary>
    string ImageUrl { get; }

    /// <summary>
    /// The current quantity of this sealed product in the user's collection.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// The timestamp when this collection entry was last updated in ISO format.
    /// </summary>
    string UpdatedAt { get; }
}
