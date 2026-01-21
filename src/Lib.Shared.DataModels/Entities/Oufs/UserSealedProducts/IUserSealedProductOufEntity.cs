namespace Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;

/// <summary>
/// Represents the output flow entity for a user's sealed product collection entry.
/// Contains the response data after adding or updating a sealed product in the collection.
/// </summary>
public interface IUserSealedProductOufEntity : Abstractions.IOufEntity
{
    /// <summary>
    /// The unique identifier (UUID) of the sealed product that was added or updated.
    /// </summary>
    string ProductUuid { get; }

    /// <summary>
    /// The current quantity of this sealed product in the user's collection after the operation.
    /// </summary>
    int Count { get; }
}
