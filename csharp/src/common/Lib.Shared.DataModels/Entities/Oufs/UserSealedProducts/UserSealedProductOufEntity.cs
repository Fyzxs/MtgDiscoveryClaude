namespace Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;

public sealed class UserSealedProductOufEntity : IUserSealedProductOufEntity
{
    public string ProductUuid { get; init; }
    public int Count { get; init; }
}
