namespace Lib.Shared.DataModels.Entities.Args.UserSealedProducts;

/// <summary>
/// Represents the client-provided argument entity for adding or updating a user's sealed product collection entry.
/// This interface is used for GraphQL input and does NOT include UserId - the user ID is extracted from JWT claims.
/// The backend validates that the authenticated user has permission to modify the specified collection.
/// </summary>
public interface IAddUserSealedProductClientArgEntity : Abstractions.IArgEntity
{
    /// <summary>
    /// The unique identifier of the collection being modified.
    /// The backend validates that the authenticated user (from JWT) has permission to access this collection.
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
