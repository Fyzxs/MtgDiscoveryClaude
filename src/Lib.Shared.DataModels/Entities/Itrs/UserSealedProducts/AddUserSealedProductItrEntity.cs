namespace Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;

/// <summary>
/// Internal transfer entity for adding or updating a user's sealed product collection entry.
/// Implements IAddUserSealedProductItrEntity with init-only properties.
/// </summary>
public sealed class AddUserSealedProductItrEntity : IAddUserSealedProductItrEntity
{
    public string UserId { get; init; }
    public string CollectionId { get; init; }
    public string ProductUuid { get; init; }
    public string SetId { get; init; }
    public int CountDelta { get; init; }
}
