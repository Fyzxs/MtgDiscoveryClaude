namespace Lib.Adapter.UserSealedProducts.Apis.Entities;

public interface IUserSealedProductXfrEntity
{
    string UserId { get; }
    string ProductUuid { get; }
    string SetId { get; }
    int CountDelta { get; }
}
