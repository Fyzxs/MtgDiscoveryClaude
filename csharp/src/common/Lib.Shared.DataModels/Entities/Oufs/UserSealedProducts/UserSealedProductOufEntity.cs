namespace Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;

public sealed class UserSealedProductOufEntity : IUserSealedProductOufEntity
{
    public string UserId { get; init; }
    public string ProductUuid { get; init; }
    public string ProductName { get; init; }
    public string SetCode { get; init; }
    public string Category { get; init; }
    public string ImageUrl { get; init; }
    public int Count { get; init; }
    public string UpdatedAt { get; init; }
}
