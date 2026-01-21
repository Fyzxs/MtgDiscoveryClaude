namespace Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;

/// <summary>
/// Internal transfer entity for adding or updating a user's sealed product collection entry.
/// Used when a user adds, removes, or modifies their sealed product inventory.
/// Data flow: Entry → Domain → Aggregator layers
/// </summary>
public interface IAddUserSealedProductItrEntity : Abstractions.IItrEntity
{
    /// <summary>
    /// The unique identifier of the user who owns this sealed product collection entry.
    /// </summary>
    string UserId { get; }

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
