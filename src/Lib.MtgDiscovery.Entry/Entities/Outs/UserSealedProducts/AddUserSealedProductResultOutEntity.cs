namespace Lib.MtgDiscovery.Entry.Entities.Outs.UserSealedProducts;

/// <summary>
/// Output entity representing the result of adding a sealed product to the collection.
/// Contains the product UUID and updated count.
/// </summary>
public sealed class AddUserSealedProductResultOutEntity
{
    public string ProductUuid { get; init; }
    public int Count { get; init; }
}
