namespace Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;

public sealed class AddUserSealedProductItrEntity : IAddUserSealedProductItrEntity
{
    public string UserId { get; init; }
    public string ProductUuid { get; init; }
    public string SetId { get; init; }
    public int CountDelta { get; init; }
}
