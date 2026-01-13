namespace Lib.MtgDiscovery.Entry.Entities.Outs.UserSealedProducts;

/// <summary>
/// Output entity representing a user's sealed product collection entry.
/// Contains all information needed for display in the GraphQL layer.
/// </summary>
public sealed class UserSealedProductOutEntity
{
    public string CollectionId { get; init; }
    public string ProductUuid { get; init; }
    public string ProductName { get; init; }
    public string SetCode { get; init; }
    public string Category { get; init; }
    public string ImageUrl { get; init; }
    public int Count { get; init; }
    public string UpdatedAt { get; init; }
}
