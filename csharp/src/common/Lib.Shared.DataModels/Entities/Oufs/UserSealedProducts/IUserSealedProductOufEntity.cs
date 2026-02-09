namespace Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;

public interface IUserSealedProductOufEntity : Abstractions.IOufEntity
{
    string UserId { get; }
    string ProductUuid { get; }
    string ProductName { get; }
    string SetCode { get; }
    string Category { get; }
    string ImageUrl { get; }
    int Count { get; }
    string UpdatedAt { get; }
}
