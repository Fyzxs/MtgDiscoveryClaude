namespace Lib.Shared.DataModels.Entities.Args.UserSealedProducts;

/// <summary>
/// Represents the argument entity for adding or updating a user's sealed product collection entry.
/// Used when a user adds, removes, or modifies their sealed product inventory.
/// Includes both UserId (from JWT) and CollectionId (from client) for authorization validation.
/// </summary>
public interface IAddUserSealedProductArgEntity : Abstractions.IArgEntity
{
    /// <summary>
    /// The unique identifier of the authenticated user (extracted from JWT token).
    /// Used to validate that the user has permission to modify the specified collection.
    /// </summary>
    string UserId { get; }

    /// <summary>
    /// The unique identifier of the collection being modified (provided by client).
    /// The backend validates that UserId has permission to access this collection.
    /// </summary>
    string CollectionId { get; }

    /// <summary>
    /// The unique identifier (UUID) of the sealed product being added to the collection.
    /// </summary>
    string ProductUuid { get; }

    /// <summary>
    /// The identifier of the set this sealed product belongs to.
    /// </summary>
    string SetId { get; }

    /// <summary>
    /// The change in quantity for this sealed product (+1, -1, +5, etc.).
    /// Positive values add to the collection, negative values remove from the collection.
    /// </summary>
    int CountDelta { get; }
}
