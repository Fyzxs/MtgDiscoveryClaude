using Lib.Shared.DataModels.Abstractions;

namespace Lib.Adapter.UserSealedProducts.Apis.Entities;

public interface IUserSealedProductXfrEntity : IXfrEntity
{
    string UserId { get; }
    string ProductUuid { get; }
    string SetId { get; }
    int CountDelta { get; }
}
