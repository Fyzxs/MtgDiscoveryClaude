namespace Lib.Adapter.UserSealedProducts.Apis.Entities;

internal interface IUserSealedProductXfrEntity
{
    string UserId { get; }
    string ProductUuid { get; }
    string SetId { get; }
    int CountDelta { get; }
    string ProductName { get; }
    string SetCode { get; }
    string Category { get; }
    string ImageUrl { get; }
}
